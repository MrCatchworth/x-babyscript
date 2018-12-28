using System;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System.IO;
using XRebirthBabyScript.Tree;
using XRebirthBabyScript.Compile;

namespace XRebirthBabyScript.Decompile
{
    public class BabyScriptDecompiler : IBabyScriptConverter
    {
        private static readonly Regex SnakeCaseRegex = new Regex("_([a-z])");
        //to break up multiline XML comments
        private static readonly Regex NewlineRegex = new Regex("\r\n|\r|\n");
        private int indentLevel;
        private string curElementComment;
        public TextWriter Writer { get; private set; }
        public XmlReader Reader { get; private set; }
        private ConversionProperties _properties;

        public BabyScriptDecompiler()
        {
            indentLevel = 0;
            curElementComment = null;
        }

        public bool Convert(ConversionProperties properties)
        {
            _properties = properties;
            Writer = new StreamWriter(_properties.OutputStream);
            Reader = XmlReader.Create(_properties.InputStream);

            var shortcuts = new List<ElementShortcut>();
            // Set Exact
            shortcuts.Add(new ElementShortcut()
            {
                HeaderMatcher = new SetValueHeaderRule(),
                AttributeMatchers = new List<IAttributeRule> {
                        new NameAttributeRule("name"),
                        new NameAttributeRule("exact")
                    },
                Apply = dc =>
                {
                    Writer.Write(Reader.GetAttribute("name"));
                    Writer.Write(" = ");
                    Writer.Write(Reader.GetAttribute("exact"));
                    Writer.Write(";");
                }
            });

            // Increment
            shortcuts.Add(new ElementShortcut()
            {
                HeaderMatcher = new SetValueHeaderRule(),
                AttributeMatchers = new List<IAttributeRule> {
                        new NameAttributeRule("name"),
                        new ExactAttributeRule("operation", "add")
                    },
                Apply = dc =>
                {
                    Writer.Write(Reader.GetAttribute("name"));
                    Writer.Write(" ++");
                    Writer.Write(";");
                }
            });

            // Decrement
            shortcuts.Add(new ElementShortcut()
            {
                HeaderMatcher = new SetValueHeaderRule(),
                AttributeMatchers = new List<IAttributeRule> {
                        new NameAttributeRule("name"),
                        new ExactAttributeRule("operation", "subtract")
                    },
                Apply = dc =>
                {
                    Writer.Write(Reader.GetAttribute("name"));
                    Writer.Write(" --");
                    Writer.Write(";");
                }
            });

            // Addition assign
            shortcuts.Add(new ElementShortcut()
            {
                HeaderMatcher = new SetValueHeaderRule(),
                AttributeMatchers = new List<IAttributeRule> {
                        new NameAttributeRule("name"),
                        new NameAttributeRule("exact"),
                        new ExactAttributeRule("operation", "add")
                    },
                Apply = dc =>
                {
                    Writer.Write(Reader.GetAttribute("name"));
                    Writer.Write(" += ");
                    Writer.Write(Reader.GetAttribute("exact"));
                    Writer.Write(";");
                }
            });

            // Subtraction assign
            shortcuts.Add(new ElementShortcut()
            {
                HeaderMatcher = new SetValueHeaderRule(),
                AttributeMatchers = new List<IAttributeRule> {
                        new NameAttributeRule("name"),
                        new NameAttributeRule("exact"),
                        new ExactAttributeRule("operation", "subtract")
                    },
                Apply = dc =>
                {
                    Writer.Write(Reader.GetAttribute("name"));
                    Writer.Write(" -= ");
                    Writer.Write(Reader.GetAttribute("exact"));
                    Writer.Write(";");
                }
            });

            while (Reader.Read())
            {
                if (Reader.NodeType == XmlNodeType.Element)
                {

                    string shortName = properties.GetShortElementName(Reader.Name);
                    WriteIndent();

                    //write a shorthand assign statement if possible
                    var shortcutUsed = false;
                    foreach (var shortcut in shortcuts)
                    {
                        if (shortcut.CheckMatch(Reader))
                        {
                            shortcut.Apply(this);
                            shortcutUsed = true;
                            break;
                        }
                    }

                    //otherwise, write a normal element
                    if (!shortcutUsed)
                    {
                        string trueName = shortName ?? Reader.Name;
                        if (_properties.Options.ConvertCaseStyle)
                        {
                            trueName = SnakeCaseRegex.Replace(trueName, match =>
                            {
                                return match.Groups[1].Captures[0].Value.ToUpper();
                            });
                        }
                        Writer.Write(trueName);

                        if (Reader.HasAttributes)
                        {
                            int attrCount = Reader.AttributeCount;
                            Writer.Write("(");
                            WriteAttributes();
                            Writer.Write(")");
                        }

                        if (Reader.IsEmptyElement)
                        {
                            Writer.Write(";");
                        }
                    }

                    //either way write any comment as necessary
                    if (curElementComment != null)
                    {
                        Writer.Write(' ');
                        WriteComment(curElementComment);
                        curElementComment = null;
                    }

                    Writer.WriteLine();

                    if (!shortcutUsed && !Reader.IsEmptyElement)
                    {
                        WriteIndent();
                        Writer.WriteLine("{");
                        indentLevel++;
                    }
                }
                else if (Reader.NodeType == XmlNodeType.EndElement)
                {
                    indentLevel--;
                    WriteIndent();
                    Writer.WriteLine("}");
                }
                else if (Reader.NodeType == XmlNodeType.Comment)
                {
                    WriteIndent();
                    WriteComment(Reader.Value);
                    Writer.WriteLine();
                }
                else if (Reader.NodeType == XmlNodeType.Text || Reader.NodeType == XmlNodeType.Whitespace)
                {
                    int numNewlines = Reader.Value.Count(c => c == '\n');
                    for (int i = 0; i < numNewlines - 1; i++)
                    {
                        Writer.WriteLine();
                    }
                }
            }

            Writer.Flush();
            return true;
        }

        private void WriteAttributes()
        {
            if (!Reader.HasAttributes)
            {
                return;
            }

            // string[] impliedNames = attrConfig.GetAnonAttributes(reader.Name);
            _properties.ImpliedAttributeNames.TryGetValue(Reader.Name, out var impliedNames);

            List<BabyAttribute> allAttributes = new List<BabyAttribute>();
            List<BabyAttribute> namedAttributes = new List<BabyAttribute>();
            List<BabyAttribute> anonAttributes = new List<BabyAttribute>();

            //just get a list of all the attributes, in our own data structure
            while (Reader.MoveToNextAttribute())
            {
                if (Reader.Name == "comment")
                {
                    curElementComment = Reader.Value;
                    continue;
                }
                BabyAttribute newAttribute = new BabyAttribute(Reader.Name.Replace(":", ""), Reader.Value);
                allAttributes.Add(newAttribute);
            }

            //if there are some implied attributes, try to match them in order with what we get until we can't
            //since the order matters, we can't match the second implied one without having the first
            if (impliedNames != null)
            {
                foreach (string name in impliedNames)
                {
                    bool found = false;
                    foreach (BabyAttribute attrib in allAttributes)
                    {
                        if (attrib.Name == name)
                        {
                            attrib.IsAnonymous = true;
                            anonAttributes.Add(attrib);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        break;
                    }
                }
            }

            //and now, a named attribute is just an attribute we didn't find to be anonymous
            foreach (BabyAttribute attribute in allAttributes)
            {
                if (!attribute.IsAnonymous)
                {
                    namedAttributes.Add(attribute);
                }
            }

            //things to write is all the nameless ones followed by the named ones
            IEnumerable<BabyAttribute> thingsToWrite = anonAttributes.Concat(namedAttributes);

            int attrNumber = 0;
            foreach (BabyAttribute attribute in thingsToWrite)
            {
                if (!attribute.IsAnonymous)
                {
                    Writer.Write(attribute.Name);
                    Writer.Write(":");
                }

                BabyScriptLexer lexer = new BabyScriptLexer(new AntlrInputStream(attribute.Value));
                CommonTokenStream tokens = new CommonTokenStream(lexer);
                BabyScriptParser parser = new BabyScriptParser(tokens);
                parser.RemoveErrorListener(ConsoleErrorListener<IToken>.Instance);
                parser.exprEof();
                if (parser.NumberOfSyntaxErrors > 0)
                {
                    Console.Error.WriteLine("Line {0}: \"{1}\" isn't a valid expression and will be wrapped in doublequotes", ((IXmlLineInfo)Reader).LineNumber, attribute.Value);
                    Writer.Write("\"" + attribute.Value + "\"");
                }
                else
                {
                    Writer.Write(attribute.Value);
                }

                if (allAttributes.Count > 1 && attrNumber < allAttributes.Count - 1)
                {
                    Writer.Write(", ");
                }
                attrNumber++;
            }

            Reader.MoveToElement();
        }

        private void WriteIndent()
        {
            Writer.Write(new string(' ', _properties.Options.Indent * indentLevel));
        }

        private void WriteComment(string comment)
        {
            if (comment.IndexOf('\n') != -1)
            {
                Writer.Write("/*");
                Writer.Write(comment);
                Writer.Write("*/");
            }
            else
            {
                Writer.Write("//" + NewlineRegex.Replace(comment, " "));
            }
        }
    }
}
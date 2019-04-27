using System;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System.IO;
using XBabyScript.Tree;
using XBabyScript.Compile;
using XBabyScript.Writer;

namespace XBabyScript.Decompile
{
    public class BabyScriptDecompiler : IBabyScriptConverter
    {
        private static readonly Regex SnakeCaseRegex = new Regex("_([a-z])");
        private string curElementComment;
        public BabyScriptWriter Writer { get; private set; }
        public XmlReader Reader { get; private set; }
        private ConversionProperties _properties;

        public BabyScriptDecompiler()
        {
            curElementComment = null;
        }

        public bool Convert(ConversionProperties properties)
        {
            _properties = properties;
            Writer = new BabyScriptWriter(_properties.OutputStream);
            Reader = XmlReader.Create(_properties.InputStream);

            var shortcuts = new List<ElementShortcut>();
            // Set Exact
            shortcuts.Add(new ElementShortcut()
            {
                HeaderRule = new SetValueHeaderRule(),
                AttributeRules = new List<IAttributeRule> {
                        new NameAttributeRule("name"),
                        new NameAttributeRule("exact")
                    },
                Apply = dc =>
                {
                    Writer.WriteAssign(
                        Reader.GetAttribute("name"),
                        Reader.GetAttribute("exact")
                    );
                }
            });

            // Increment
            shortcuts.Add(new ElementShortcut()
            {
                HeaderRule = new SetValueHeaderRule(),
                AttributeRules = new List<IAttributeRule> {
                        new NameAttributeRule("name"),
                        new ExactAttributeRule("operation", "add")
                    },
                Apply = dc =>
                {
                    Writer.WriteIncrement(Reader.GetAttribute("name"));
                }
            });

            // Decrement
            shortcuts.Add(new ElementShortcut()
            {
                HeaderRule = new SetValueHeaderRule(),
                AttributeRules = new List<IAttributeRule> {
                        new NameAttributeRule("name"),
                        new ExactAttributeRule("operation", "subtract")
                    },
                Apply = dc =>
                {
                    Writer.WriteDecrement(Reader.GetAttribute("name"));
                }
            });

            // Addition assign
            shortcuts.Add(new ElementShortcut()
            {
                HeaderRule = new SetValueHeaderRule(),
                AttributeRules = new List<IAttributeRule> {
                        new NameAttributeRule("name"),
                        new NameAttributeRule("exact"),
                        new ExactAttributeRule("operation", "add")
                    },
                Apply = dc =>
                {
                    Writer.WriteAssign(
                        Reader.GetAttribute("name"),
                        Reader.GetAttribute("exact"),
                        BabyAssignType.Add
                    );
                }
            });

            // Subtraction assign
            shortcuts.Add(new ElementShortcut()
            {
                HeaderRule = new SetValueHeaderRule(),
                AttributeRules = new List<IAttributeRule> {
                        new NameAttributeRule("name"),
                        new NameAttributeRule("exact"),
                        new ExactAttributeRule("operation", "subtract")
                    },
                Apply = dc =>
                {
                    Writer.WriteAssign(
                        Reader.GetAttribute("name"),
                        Reader.GetAttribute("exact"),
                        BabyAssignType.Subtract
                    );
                }
            });

            // Delete
            shortcuts.Add(new ElementShortcut()
            {
                HeaderRule = new RemoveValueHeaderRule(),
                AttributeRules = new List<IAttributeRule> {
                        new NameAttributeRule("name")
                    },
                Apply = dc =>
                {
                    Writer.WriteDelete(
                        Reader.GetAttribute("name")
                    );
                }
            });

            try
            {
                while (Reader.Read())
                {
                    if (Reader.NodeType == XmlNodeType.Element)
                    {

                        string shortName = properties.GetShortElementName(Reader.Name);

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

                            Writer.WriteElementStart(trueName);
                            if (Reader.HasAttributes)
                            {
                                WriteAttributes();
                            }

                            if (Reader.IsEmptyElement)
                            {
                                Writer.WriteElementEnd();
                            }
                        }

                        if (!Reader.IsEmptyElement)
                        {
                            Writer.WriteElementChildrenStart();
                        }

                        //either way write any comment as necessary
                        if (curElementComment != null)
                        {
                            Writer.WriteSlashComment(curElementComment);
                            curElementComment = null;
                        }
                    }
                    else if (Reader.NodeType == XmlNodeType.EndElement)
                    {
                        Writer.WriteElementEnd();
                    }
                    else if (Reader.NodeType == XmlNodeType.Comment)
                    {
                        Writer.WriteComment(Reader.Value);
                    }
                    else if (Reader.NodeType == XmlNodeType.Whitespace)
                    {
                        var numNewlines = Reader.Value.Count(c => c == '\n');
                        if (numNewlines > 1)
                        {
                            Writer.WriteEmptyLines(numNewlines - 1);
                        }
                    }
                    else if (Reader.NodeType == XmlNodeType.Text)
                    {
                        Writer.WriteTextNode(Reader.Value.Trim());
                    }
                }
            }
            catch (XmlException e)
            {
                _properties.Logger.LogError(_properties.FileName, e.LineNumber, e.LinePosition, e.Message);
                return false;
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

            var allAttributes = new List<BabyAttribute>();
            var namedAttributes = new List<BabyAttribute>();
            var anonAttributes = new List<BabyAttribute>();

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

            foreach (BabyAttribute attribute in thingsToWrite)
            {
                BabyScriptLexer lexer = new BabyScriptLexer(new AntlrInputStream(attribute.Value));
                CommonTokenStream tokens = new CommonTokenStream(lexer);
                BabyScriptParser parser = new BabyScriptParser(tokens);
                parser.RemoveErrorListener(ConsoleErrorListener<IToken>.Instance);
                parser.exprEof();

                var isValidExpression = parser.NumberOfSyntaxErrors == 0;
                if (!isValidExpression)
                {
                    Console.Error.WriteLine("Line {0}: \"{1}\" isn't a valid expression and will be wrapped in doublequotes", ((IXmlLineInfo)Reader).LineNumber, attribute.Value);
                }

                Writer.WriteAttribute(
                    isValidExpression ? attribute.Name : null,
                    attribute.Value,
                    !isValidExpression
                );
            }

            Reader.MoveToElement();
        }
    }
}
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
        private TextWriter writer;
        private XmlReader reader;
        private ConversionProperties _properties;

        public BabyScriptDecompiler()
        {
            indentLevel = 0;
            curElementComment = null;
        }

        public bool Convert(ConversionProperties properties)
        {
            _properties = properties;
            writer = new StreamWriter(_properties.OutputStream);
            reader = XmlReader.Create(_properties.InputStream);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {

                    string shortName = properties.GetShortElementName(reader.Name);
                    WriteIndent();

                    //write a shorthand assign statement if possible
                    bool shortcutUsed = TryAssignmentShortcut() || TryIncrementShortcut() || TryDecrementShortcut();

                    //otherwise, write a normal element
                    if (!shortcutUsed)
                    {
                        string trueName = shortName ?? reader.Name;
                        if (_properties.Options.ConvertCaseStyle) {
                            trueName = SnakeCaseRegex.Replace(trueName, match => {
                                return match.Groups[1].Captures[0].Value.ToUpper();
                            });
                        }
                        writer.Write(trueName);

                        if (reader.HasAttributes)
                        {
                            int attrCount = reader.AttributeCount;
                            writer.Write("(");
                            WriteAttributes();
                            writer.Write(")");
                        }

                        if (reader.IsEmptyElement)
                        {
                            writer.Write(";");
                        }
                    }

                    //either way write any comment as necessary
                    if (curElementComment != null)
                    {
                        writer.Write(' ');
                        WriteComment(curElementComment);
                        curElementComment = null;
                    }

                    writer.WriteLine();

                    if (!shortcutUsed && !reader.IsEmptyElement)
                    {
                        WriteIndent();
                        writer.WriteLine("{");
                        indentLevel++;
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    indentLevel--;
                    WriteIndent();
                    writer.WriteLine("}");
                }
                else if (reader.NodeType == XmlNodeType.Comment)
                {
                    WriteIndent();
                    WriteComment(reader.Value);
                    writer.WriteLine();
                }
                else if (reader.NodeType == XmlNodeType.Text || reader.NodeType == XmlNodeType.Whitespace)
                {
                    int numNewlines = reader.Value.Count(c => c == '\n');
                    for (int i = 0; i < numNewlines - 1; i++)
                    {
                        writer.WriteLine();
                    }
                }
            }

            writer.Flush();
            return true;
        }

        private bool TryAssignmentShortcut()
        {
            if (reader.Name != "set_value")
            {
                return false;
            }
            if (!reader.IsEmptyElement)
            {
                return false;
            }

            string varOperation = null;
            string varName = null;

            while (reader.MoveToNextAttribute())
            {
                if (reader.Name == "name")
                {
                    varName = reader.Value;
                }
                else if (reader.Name == "exact")
                {
                    varOperation = reader.Value;
                }
                else if (reader.Name == "comment")
                {
                    curElementComment = reader.Value;
                }
                else
                {
                    reader.MoveToElement();
                    return false;
                }
            }

            if (varOperation == null || varName == null)
            {
                reader.MoveToElement();
                return false;
            }

            writer.Write(varName);
            writer.Write(" = ");
            writer.Write(varOperation);
            writer.Write(";");
            return true;
        }
        private bool TryIncrementShortcut()
        {
            if (reader.Name != "set_value")
            {
                return false;
            }
            if (!reader.IsEmptyElement)
            {
                return false;
            }

            string varOperation = null;
            string varName = null;

            while (reader.MoveToNextAttribute())
            {
                if (reader.Name == "name")
                {
                    varName = reader.Value;
                }
                else if (reader.Name == "operation" && reader.Value == "add")
                {
                    varOperation = reader.Value;
                }
                else if (reader.Name == "comment")
                {
                    curElementComment = reader.Value;
                }
                else
                {
                    reader.MoveToElement();
                    return false;
                }
            }

            if (varOperation == null || varName == null)
            {
                reader.MoveToElement();
                return false;
            }

            writer.Write(varName);
            writer.Write(" ++");
            writer.Write(";");
            return true;
        }

        private bool TryDecrementShortcut()
        {
            if (reader.Name != "set_value")
            {
                return false;
            }
            if (!reader.IsEmptyElement)
            {
                return false;
            }

            string varOperation = null;
            string varName = null;

            while (reader.MoveToNextAttribute())
            {
                if (reader.Name == "name")
                {
                    varName = reader.Value;
                }
                else if (reader.Name == "operation" && reader.Value == "subtract")
                {
                    varOperation = reader.Value;
                }
                else if (reader.Name == "comment")
                {
                    curElementComment = reader.Value;
                }
                else
                {
                    reader.MoveToElement();
                    return false;
                }
            }

            if (varOperation == null || varName == null)
            {
                reader.MoveToElement();
                return false;
            }

            writer.Write(varName);
            writer.Write(" --");
            writer.Write(";");
            return true;
        }

        private void WriteAttributes()
        {
            if (!reader.HasAttributes)
            {
                return;
            }

            // string[] impliedNames = attrConfig.GetAnonAttributes(reader.Name);
            _properties.ImpliedAttributeNames.TryGetValue(reader.Name, out var impliedNames);

            List<BabyAttribute> allAttributes = new List<BabyAttribute>();
            List<BabyAttribute> namedAttributes = new List<BabyAttribute>();
            List<BabyAttribute> anonAttributes = new List<BabyAttribute>();

            //just get a list of all the attributes, in our own data structure
            while (reader.MoveToNextAttribute())
            {
                if (reader.Name == "comment")
                {
                    curElementComment = reader.Value;
                    continue;
                }
                BabyAttribute newAttribute = new BabyAttribute(reader.Name.Replace(":", ""), reader.Value);
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
                    writer.Write(attribute.Name);
                    writer.Write(":");
                }

                BabyScriptLexer lexer = new BabyScriptLexer(new AntlrInputStream(attribute.Value));
                CommonTokenStream tokens = new CommonTokenStream(lexer);
                BabyScriptParser parser = new BabyScriptParser(tokens);
                parser.RemoveErrorListener(ConsoleErrorListener<IToken>.Instance);
                parser.exprEof();
                if (parser.NumberOfSyntaxErrors > 0)
                {
                    Console.Error.WriteLine("Line {0}: \"{1}\" isn't a valid expression and will be wrapped in doublequotes", ((IXmlLineInfo)reader).LineNumber, attribute.Value);
                    writer.Write("\"" + attribute.Value + "\"");
                }
                else
                {
                    writer.Write(attribute.Value);
                }

                if (allAttributes.Count > 1 && attrNumber < allAttributes.Count - 1)
                {
                    writer.Write(", ");
                }
                attrNumber++;
            }

            reader.MoveToElement();
        }

        private void WriteIndent()
        {
            writer.Write(new string(' ', _properties.Options.Indent * indentLevel));
        }

        private void WriteComment(string comment)
        {
            if (comment.IndexOf('\n') != -1)
            {
                writer.Write("/*");
                writer.Write(comment);
                writer.Write("*/");
            }
            else
            {
                writer.Write("//" + NewlineRegex.Replace(comment, " "));
            }
        }
    }
}
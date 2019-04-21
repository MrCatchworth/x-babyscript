using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using XBabyScript.Tree;

namespace XBabyScript.Compile
{
    public class BabyScriptCompiler : IBabyScriptConverter
    {
        private static readonly Regex CamelCaseRegex = new Regex("(?<=[a-z])([A-Z])");
        public static readonly Regex IdRegex = new Regex(@"^\$?[A-Za-z][A-Za-z0-9_]*$");
        private static string GetRuleFullText(ParserRuleContext context)
        {
            var stream = context.Start.InputStream;
            return stream.GetText(new Interval(context.Start.StartIndex, context.Stop.StopIndex));
        }

        private static string ParseDoubleQuoteString(string doubleQuoteString)
        {
            var quotesStripped = doubleQuoteString.Substring(1, doubleQuoteString.Length - 2);
            return EscapeCode.Unescape(quotesStripped, '"');
        }

        public class XmlWritingListener : BabyScriptParserBaseListener
        {
            private readonly XmlWriter Writer;
            private BabyScriptParser.ElementContext CurrentElement;
            public bool Error { get; private set; }

            private readonly ConversionProperties _properties;
            private readonly CommonTokenStream _tokenStream;

            Queue<string> AvailableNames = new Queue<string>();
            bool HasImpliedAttributes;

            public readonly ICollection<SemanticError> Errors;

            public XmlWritingListener(ConversionProperties properties, CommonTokenStream tokenStream)
            {
                _properties = properties;
                _tokenStream = tokenStream;
                Writer = XmlWriter.Create(_properties.OutputStream, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = new string(' ', _properties.Options.Indent)
                });
                Error = false;
                Errors = new List<SemanticError>();
            }

            private void SetCurrentElement(BabyScriptParser.ElementContext ctx, string realName)
            {
                HasImpliedAttributes = _properties.ImpliedAttributeNames.TryGetValue(realName, out var impliedAttributes);

                AvailableNames.Clear();
                if (HasImpliedAttributes)
                {
                    foreach (string name in impliedAttributes)
                    {
                        AvailableNames.Enqueue(name);
                    }
                }

                CurrentElement = ctx;
            }

            public override void EnterSlashComment(BabyScriptParser.SlashCommentContext context)
            {
                var commentText = context.commentText.Text.Substring(2);
                Writer.WriteComment(commentText);
            }

            public override void EnterBlockComment(BabyScriptParser.BlockCommentContext context)
            {
                var commentText = context.commentText.Text.Substring(2, context.commentText.Text.Length - 4);
                Writer.WriteComment(commentText);
            }

            public override void EnterElement(BabyScriptParser.ElementContext ctx)
            {
                string realName = ctx.eleName.GetText();
                string fullName = _properties.GetFullElementName(realName);
                if (fullName != null)
                {
                    realName = fullName;
                }
                realName = CamelCaseRegex.Replace(realName, match =>
                {
                    return "_" + match.Groups[1].Captures[0].Value.ToLower();
                });

                if (Error) return;

                Writer.WriteStartElement(realName);
                SetCurrentElement(ctx, realName);
            }

            public override void ExitElement(BabyScriptParser.ElementContext ctx)
            {
                if (Error) return;
                Writer.WriteEndElement();
            }

            public override void EnterAssign(BabyScriptParser.AssignContext context)
            {
                if (Error) return;

                Writer.WriteStartElement("set_value");
                Writer.WriteAttributeString("name", GetRuleFullText(context.leftHand));
                Writer.WriteAttributeString("exact", GetRuleFullText(context.rightHand));
                Writer.WriteEndElement();
            }

            public override void EnterIncrement(BabyScriptParser.IncrementContext context)
            {
                if (Error) return;

                Writer.WriteStartElement("set_value");
                Writer.WriteAttributeString("name", GetRuleFullText(context.leftHand));
                Writer.WriteAttributeString("operation", "add");
                Writer.WriteEndElement();
            }

            public override void EnterDecrement(BabyScriptParser.DecrementContext context)
            {
                if (Error) return;

                Writer.WriteStartElement("set_value");
                Writer.WriteAttributeString("name", GetRuleFullText(context.leftHand));
                Writer.WriteAttributeString("operation", "subtract");
                Writer.WriteEndElement();
            }

            public override void EnterAdditionAssign(BabyScriptParser.AdditionAssignContext context)
            {
                if (Error) return;

                Writer.WriteStartElement("set_value");
                Writer.WriteAttributeString("name", GetRuleFullText(context.leftHand));
                Writer.WriteAttributeString("exact", GetRuleFullText(context.rightHand));
                Writer.WriteAttributeString("operation", "add");
                Writer.WriteEndElement();
            }

            public override void EnterSubtractionAssign(BabyScriptParser.SubtractionAssignContext context)
            {
                if (Error) return;

                Writer.WriteStartElement("set_value");
                Writer.WriteAttributeString("name", GetRuleFullText(context.leftHand));
                Writer.WriteAttributeString("exact", GetRuleFullText(context.rightHand));
                Writer.WriteAttributeString("operation", "subtract");
                Writer.WriteEndElement();
            }

            public override void EnterDelete(BabyScriptParser.DeleteContext context)
            {
                if (Error) return;

                Writer.WriteStartElement("remove_value");
                Writer.WriteAttributeString("name", GetRuleFullText(context.leftHand));
                Writer.WriteEndElement();
            }

            public override void EnterText(BabyScriptParser.TextContext context)
            {
                if (Error) return;

                try
                {
                    Writer.WriteString(ParseDoubleQuoteString(context.textValue.Text));
                }
                catch (ArgumentException e)
                {
                    Errors.Add(new SemanticError(
                        _properties.FileName,
                        context.Start,
                        e.Message
                    ));
                    Error = true;
                }
            }

            public override void EnterAttribute(BabyScriptParser.AttributeContext context)
            {
                var name = context.attrName?.Text;
                if (name == null)
                {
                    if (!HasImpliedAttributes)
                    {
                        Errors.Add(new SemanticError(
                            _properties.FileName,
                            context.Start,
                            $"No implied attribute names available for {CurrentElement.eleName.GetText()}"
                        ));

                        Error = true;
                    }
                    else if (AvailableNames.Count == 0)
                    {
                        Errors.Add(new SemanticError(
                            _properties.FileName,
                            context.Start,
                            $"Too many anonymous attributes defined for {CurrentElement.eleName.GetText()}"
                        ));

                        Error = true;
                    }
                    else
                    {
                        name = AvailableNames.Dequeue();
                    }
                }

                if (Error) return;

                string realValue = null;
                if (context.value.exprLiteral != null)
                {
                    try
                    {
                        var literalText = context.value.exprLiteral.Text;
                        realValue = ParseDoubleQuoteString(literalText);
                    }
                    catch (ArgumentException e)
                    {
                        Errors.Add(new SemanticError(
                            _properties.FileName,
                            context.Start,
                            e.Message
                        ));
                        Error = true;
                    }
                }
                else
                {
                    realValue = GetRuleFullText(context.value.exprValue);
                }

                Writer.WriteAttributeString(name, realValue);
            }

            public void Flush()
            {
                Writer.Flush();
            }
        }

        private class MyErrorListener : BaseErrorListener
        {
            private readonly ConversionProperties _properties;
            public MyErrorListener(ConversionProperties properties)
            {
                _properties = properties;
            }

            public override void SyntaxError(TextWriter writer, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
            {
                Console.Error.WriteLine("The type of this token is: " + recognizer.Vocabulary.GetSymbolicName(offendingSymbol.Type));
                Console.Error.WriteLine("The text of this token is: " + offendingSymbol.Text);
            }
        }

        public bool Convert(ConversionProperties properties)
        {
            var lexer = new BabyScriptLexer(new AntlrInputStream(properties.InputStream));
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new BabyScriptParser(tokenStream);
            parser.AddErrorListener(new MyErrorListener(properties));

            BabyScriptParser.DocumentContext documentContext = parser.document();

            if (parser.NumberOfSyntaxErrors > 0)
            {
                properties.Logger.LogError(properties.FileName, "Conversion failed due to syntax error(s)");
                return false;
            }

            XmlWritingListener listener = new XmlWritingListener(properties, tokenStream);
            new ParseTreeWalker().Walk(listener, documentContext);
            listener.Flush();

            if (listener.Error)
            {
                properties.Logger.LogError(properties.FileName, "Conversion failed due to semantic error(s): ");
                foreach (var error in listener.Errors)
                {
                    properties.Logger.LogError(properties.FileName, error.OffendingToken.Line, error.OffendingToken.Column, error.Message);
                }
                return false;
            }

            return true;
        }
    }
}

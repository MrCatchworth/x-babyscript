using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using XRebirthBabyScript.Tree;

namespace XRebirthBabyScript.Compile
{
    public class BabyScriptCompiler : IBabyScriptConverter
    {
        public class XmlWritingListener : BabyScriptBaseListener
        {
            private readonly XmlWriter Writer;
            private BabyScriptParser.ElementContext CurrentElement;
            public bool Error { get; private set; }

            private readonly ConversionProperties _properties;

            Queue<string> AvailableNames = new Queue<string>();
            bool HasImpliedAttributes;

            public readonly ICollection<SemanticError> Errors;

            public XmlWritingListener(ConversionProperties properties)
            {
                _properties = properties;
                Writer = XmlWriter.Create(_properties.OutputStream, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  "
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

            public override void EnterComment(BabyScriptParser.CommentContext ctx)
            {
                Writer.WriteComment(((BabyComment)ctx.treeNode).Text);
            }

            public override void EnterElement(BabyScriptParser.ElementContext ctx)
            {
                var element = (BabyElement)ctx.treeNode;
                string realName = element.Name;
                string fullName = _properties.GetFullElementName(realName);
                if (fullName != null)
                {
                    realName = fullName;
                }

                if (Error) return;

                Writer.WriteStartElement(realName);
                SetCurrentElement(ctx, realName);

                if (element.Type == ElementType.Assignment)
                {
                    foreach (BabyAttribute a in element.Attributes)
                    {
                        Writer.WriteAttributeString(a.Name, a.Value);
                    }
                }
            }

            public override void ExitElement(BabyScriptParser.ElementContext ctx)
            {
                if (Error) return;
                Writer.WriteEndElement();
            }

            public override void EnterAttribute(BabyScriptParser.AttributeContext ctx)
            {
                string name = ctx.attr.Name;
                var element = CurrentElement.treeNode as BabyElement;
                if (name == null)
                {
                    if (!HasImpliedAttributes)
                    {
                        Errors.Add(new SemanticError(_properties.FileName, ctx.Start, $"No implied attribute names available for {element.Name}"));
                        
                        Error = true;
                    }
                    else if (AvailableNames.Count == 0)
                    {
                        Errors.Add(new SemanticError(_properties.FileName, ctx.Start, $"Too many anonymous attributes defined for {element.Name}"));
                        Error = true;
                    }
                    else
                    {
                        name = AvailableNames.Dequeue();
                    }
                }

                if (Error) return;

                string realValue = ctx.attr.Value;
                //if the attribute had to packed in double quotes, time to unpack it again
                if (realValue.Length > 1 && realValue[0] == '"')
                {
                    realValue = realValue.Substring(1, realValue.Length - 2);
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
            BabyScriptLexer lex = new BabyScriptLexer(new AntlrInputStream(properties.InputStream));
            CommonTokenStream tokenStream = new CommonTokenStream(lex);
            BabyScriptParser parser = new BabyScriptParser(tokenStream);
            parser.AddErrorListener(new MyErrorListener(properties));

            BabyScriptParser.DocumentContext doc = parser.document();

            if (parser.NumberOfSyntaxErrors > 0)
            {
                properties.Logger.LogError(properties.FileName, "Conversion failed due to syntax error(s)");
                return false;
            }

            XmlWritingListener listener = new XmlWritingListener(properties);
            new ParseTreeWalker().Walk(listener, doc);
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

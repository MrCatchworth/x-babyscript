using System;
using System.IO;
using System.Linq;

namespace XBabyScript.Writer
{
    using System.Text.RegularExpressions;
    using Compile;
    using XBabyScript.Tree;

    /// <summary>
    /// A class to aid in writing valid BabyScript syntax to a stream.
    /// </summary>
    public class BabyScriptWriter
    {
        private static readonly Regex NewlineRegex = new Regex("\r\n|\r|\n");

        private enum WriterStatus
        {
            Start,
            AfterElementName,
            ElementContents,
            AttributeList,
            AfterAttributeList,
            Closed,
            Error
        }

        private readonly Stream stream;
        private readonly FormattedTokenWriter writer;
        private WriterStatus state;
        private readonly BabyScriptWriterSettings settings;
        private bool attributeWritten;


        public BabyScriptWriter(Stream stream)
        {
            this.stream = stream;
            this.state = WriterStatus.Start;
            this.settings = new BabyScriptWriterSettings()
            {
                IndentWidth = 4,
                OpenBraceOnNewLine = true
            };
            this.writer = new FormattedTokenWriter(this.stream, this.settings);
            this.attributeWritten = false;
        }

        private void AssertIsValidId(string value)
        {
            if (!BabyScriptCompiler.IdRegex.Match(value).Success)
            {
                throw new ArgumentException($"{value} is not a valid BabyScript id token");
            }
        }

        private void AssertSaneState()
        {
            if (state == WriterStatus.Closed)
            {
                state = WriterStatus.Error;
                throw new InvalidOperationException($"Cannot write to a {GetType().Name} when it is already closed.");
            }
            else if (state == WriterStatus.Error)
            {
                throw new InvalidOperationException($"Cannot write to a {GetType().Name} when it is in an error state.");
            }
        }

        private void AssertValidState(params WriterStatus[] validStatuses)
        {
            if (!validStatuses.Contains(state))
            {
                throw new InvalidOperationException($"Cannot write this at state {state} - must be any of {string.Join(", ", validStatuses)}.");
            }
        }

        private void WriteStartAttributeList()
        {
            AssertValidState(WriterStatus.AfterElementName);

            writer.WriteToken("(", false);
            state = WriterStatus.AttributeList;
        }

        private void WriteEndAttributeList()
        {
            AssertValidState(WriterStatus.AttributeList);

            writer.WriteToken(")", false);
            state = WriterStatus.AfterAttributeList;
            attributeWritten = false;
        }

        private void WriteElementEmptyChildren()
        {
            AssertValidState(
                WriterStatus.AfterElementName,
                WriterStatus.AttributeList,
                WriterStatus.AfterAttributeList
            );

            if (state == WriterStatus.AttributeList)
            {
                WriteEndAttributeList();
            }

            writer.WriteToken(";", false);
            state = WriterStatus.ElementContents;
        }

        /// <summary>
        /// Write the start of a new element with its name.
        /// After writing an element's name with this method, you can write its attributes and/or children.
        /// </summary>
        /// <param name="name">The element name to write. Must be a valid BabyScript id token.</param>
        public void WriteElementStart(string name)
        {
            AssertSaneState();
            AssertValidState(
                WriterStatus.Start,
                WriterStatus.AttributeList,
                WriterStatus.ElementContents,
                WriterStatus.AfterAttributeList,
                WriterStatus.AfterElementName
            );
            AssertIsValidId(name);

            if (state == WriterStatus.AttributeList)
            {
                WriteEndAttributeList();
                WriteElementChildrenStart();
            }
            else if (state == WriterStatus.AfterElementName || state == WriterStatus.AfterAttributeList)
            {
                WriteElementChildrenStart();
            }

            writer.WriteToken(name, true);

            state = WriterStatus.AfterElementName;
        }

        /// <summary>
        /// Write the start of an element's list of children.
        /// </summary>
        public void WriteElementChildrenStart()
        {
            AssertValidState(
                WriterStatus.AfterElementName,
                WriterStatus.AttributeList,
                WriterStatus.AfterAttributeList
            );

            if (state == WriterStatus.AttributeList)
            {
                WriteEndAttributeList();
            }

            if (settings.OpenBraceOnNewLine)
            {
                writer.WriteToken("{", true);
            }
            else
            {
                writer.WriteToken(" {", false);
            }

            state = WriterStatus.ElementContents;
            writer.IndentDepth++;
        }

        /// <summary>
        /// Close off the current element.
        /// If you have written all or part of an element's header, it will be ended with empty children.
        /// Otherwise it will close off the current block of element children.
        /// </summary>
        public void WriteElementEnd()
        {
            AssertSaneState();
            AssertValidState(
                WriterStatus.AfterElementName,
                WriterStatus.AttributeList,
                WriterStatus.AfterAttributeList,
                WriterStatus.ElementContents
            );

            if (state == WriterStatus.AfterElementName || state == WriterStatus.AfterAttributeList)
            {
                WriteElementEmptyChildren();
                state = WriterStatus.ElementContents;
            }
            else if (state == WriterStatus.AttributeList)
            {
                WriteEndAttributeList();
                WriteElementEmptyChildren();
                state = WriterStatus.ElementContents;
            }
            else
            {
                if (writer.IndentDepth <= 0) {
                    throw new InvalidOperationException("Tried to close an element's children, but we are at the top level of the document");
                }

                writer.IndentDepth--;
                writer.WriteToken("}", true);
            }
        }

        /// <summary>
        /// Write an element attribute.
        /// You must have just written an element's name, or have already started its attribute list.
        /// </summary>
        /// <param name="name">Name of the attribute. If null or empty, it is an anonymous attribute, and no name is written.</param>
        /// <param name="value">Value of the attribute.</param>
        /// <param name="isStringValue">If true, the attribute's value is written as a doublequote string instead of an expression. Defaults to false.</param>
        public void WriteAttribute(string name, string value, bool isStringValue = false) {
            AssertSaneState();
            AssertValidState(
                WriterStatus.AfterElementName,
                WriterStatus.AttributeList
            );

            if (isStringValue)
            {
                value = CreateDoubleQuoteString(value);
            }

            if (state == WriterStatus.AfterElementName)
            {
                WriteStartAttributeList();
            }

            if (attributeWritten)
            {
                writer.WriteToken(", ", false);
            }

            if (!string.IsNullOrEmpty(name))
            {
                writer.WriteToken(name, false);
                writer.WriteToken(":", false);
            }
            writer.WriteToken(value, false);

            attributeWritten = true;
        }

        /// <summary>
        /// Write an assignment statement. Any open element header will be closed off automatically.
        /// </summary>
        /// <param name="leftHand">Left hand of the assignment.</param>
        /// <param name="rightHand">Right hand of the assignment.</param>
        /// <param name="assignType">What kind of assignment statement it is. Defaults to a normal assignment.</param>
        public void WriteAssign(string leftHand, string rightHand, BabyAssignType assignType = BabyAssignType.Normal)
        {
            AssertSaneState();
            if (IsElementHeaderInProgress())
            {
                WriteElementEnd();
            }

            writer.WriteToken(leftHand, true);

            if (assignType == BabyAssignType.Add)
            {
                writer.WriteToken(" += ", false);
            }
            else if (assignType == BabyAssignType.Subtract)
            {
                writer.WriteToken(" -= ", false);
            }
            else
            {   
                writer.WriteToken(" = ", false);
            }

            writer.WriteToken(rightHand, false);
            writer.WriteToken(";", false);
        }

        /// <summary>
        /// Write an increment statement.
        /// </summary>
        /// <param name="value">Expression value to increment.</param>
        public void WriteIncrement(string value)
        {
            AssertSaneState();
            if (IsElementHeaderInProgress())
            {
                WriteElementEnd();
            }
            writer.WriteToken(value, true);
            writer.WriteToken(" ++;", false);
        }

        /// <summary>
        /// Write a decrement statement.
        /// </summary>
        /// <param name="value">Expression value to decrement.</param>
        public void WriteDecrement(string value)
        {
            AssertSaneState();
            if (IsElementHeaderInProgress())
            {
                WriteElementEnd();
            }
            writer.WriteToken(value, true);
            writer.WriteToken(" --;", false);
        }

        public void WriteDelete(string value)
        {
            AssertSaneState();
            if (IsElementHeaderInProgress())
            {
                WriteElementEnd();
            }

            writer.WriteToken("delete ", true);
            writer.WriteToken(value, false);
            writer.WriteToken(";", false);
        }

        private string CreateDoubleQuoteString(string value)
        {
            var textValue = EscapeCode.Escape(value.Trim(), '"');
            return "\"" + textValue + "\"";
        }

        /// <summary>
        /// Write a text node in the form of a doublequote string.
        /// </summary>
        /// <param name="value">Inner text of the text node to write.</param>
        public void WriteTextNode(string value)
        {
            writer.WriteToken(CreateDoubleQuoteString(value), true);
            writer.WriteToken(";", false);
        }

        public void Flush()
        {
            writer.Flush();
        }

        private bool IsElementHeaderInProgress()
        {
            return (
                state == WriterStatus.AfterElementName ||
                state == WriterStatus.AttributeList ||
                state == WriterStatus.AfterAttributeList
            );
        }


        /// <summary>
        /// Write a comment node. If the comment contains line breaks it will be written as a block comment;
        /// otherwise, as a slash comment.
        /// When a slash comment is written using this method, it is always on a new line.
        /// See <see cref="WriteBlockComment" /> or <see cref="WriteSlashComment" />.
        /// </summary>
        /// <param name="value">Comment text to write.</param>
        public void WriteComment(string value)
        {
            if (value.IndexOf('\n') != -1)
            {
                WriteBlockComment(value);
            }
            else
            {
                WriteSlashComment(value, true);
            }
        }

        /// <summary>
        /// Write a block comment node.
        /// The comment's text will be mutated so that each line of indentation is consistent with the current formatting.
        /// </summary>
        /// <param name="value">Comment text to write.</param>
        public void WriteBlockComment(string value)
        {
            AssertSaneState();
            AssertValidState(
                WriterStatus.Start,
                WriterStatus.ElementContents
            );

            writer.WriteToken("/*", true);
            writer.WriteLine();
            writer.WriteRawString(FixCommentIndent(value));
            writer.WriteToken("*/", true);
            writer.WriteLine();
        }

        /// <summary>
        /// Write a slash comment. The comment text must not contain a line break.
        /// A line break is always written after the comment.
        /// </summary>
        /// <param name="value">Comment text to write.</param>
        /// <param name="newLine">Whether or not the comment should go on a new line. Defaults to false.</param>
        public void WriteSlashComment(string value, bool newLine = false)
        {
            AssertSaneState();
            AssertValidState(
                WriterStatus.Start,
                WriterStatus.ElementContents
            );

            if (NewlineRegex.IsMatch(value))
            {
                throw new ArgumentException("Cannot write slash comments which contain a line break");
            }

            if (writer.LineIsDirty)
            {
                writer.WriteToken(" ", false);
            }
            writer.WriteToken("//" + value, newLine);
            writer.WriteLine();
        }

        private string FixCommentIndent(string commentText)
        {
            var commentLines = NewlineRegex.Split(commentText);
            var indentString = new string(' ', (writer.IndentDepth + 1) * settings.IndentWidth);
            for (int i = 0; i < commentLines.Length; i++)
            {
                var rawLine = commentLines[i];
                commentLines[i] = indentString + rawLine.TrimStart();
            }

            return string.Join("\n", commentLines);
        }

        /// <summary>
        /// Write a line break.
        /// </summary>
        public void WriteLine()
        {
            writer.WriteLine();
        }

        public void WriteEmptyLines(int amount)
        {
            writer.WriteEmptyLines(amount);
        }
    }
}
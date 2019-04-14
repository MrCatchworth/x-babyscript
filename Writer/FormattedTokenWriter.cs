using System.Text.RegularExpressions;
using System;
using System.IO;

namespace XBabyScript.Writer
{
    internal class FormattedTokenWriter
    {
        private static readonly Regex EndsWithNewlineRegex = new Regex(@"(\r\n|\r|\n)$");
        private static readonly Regex TokenForbiddenCharsRegex = new Regex(@"[\r\n]");
        private readonly FormattedTokenWriterSettings settings;
        private readonly Stream stream;
        private readonly TextWriter writer;
        private bool lineIsDirty;
        private int _indentDepth;
        public int IndentDepth
        {
            get
            {
                return _indentDepth;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Cannot set indent depth to be less than zero");
                }
                _indentDepth = value;
            }
        }

        public bool LineIsDirty => lineIsDirty;

        internal FormattedTokenWriter(Stream _stream, FormattedTokenWriterSettings _settings)
        {
            settings = _settings;
            stream = _stream;
            writer = new StreamWriter(stream);
            lineIsDirty = false;
            IndentDepth = 0;
        }

        private void WriteIndent()
        {
            if (IndentDepth > 0)
            {
                writer.Write(new string(' ', IndentDepth * settings.IndentWidth));
                lineIsDirty = true;
            }
        }

        public void WriteToken(string token, bool newLine)
        {
            if (TokenForbiddenCharsRegex.IsMatch(token))
            {
                throw new ArgumentException($"Tried to write token {token}, which contains forbidden characters");
            }
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token must not be null or empty string");
            }

            if (newLine)
            {
                WriteLine();
            }
            if (!lineIsDirty)
            {
                WriteIndent();
            }

            writer.Write(token);
            lineIsDirty = true;
        }

        public void WriteLine()
        {
            writer.WriteLine();
            lineIsDirty = false;
        }

        public void WriteRawString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            writer.Write(value);

            lineIsDirty = !EndsWithNewlineRegex.IsMatch(value);
        }

        public void Flush()
        {
            writer.Flush();
        }
    }
}
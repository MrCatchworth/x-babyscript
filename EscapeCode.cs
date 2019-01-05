using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XBabyScript
{
    public class EscapeCode
    {
        private static readonly Regex EscapeCodeRegex = new Regex(@"\\(.)");

        private static readonly List<EscapeCode> Sequences = new List<EscapeCode>
        {
            new EscapeCode('\\', "\\"),
            new EscapeCode('n', "\n"),
            new EscapeCode('t', "\t"),
            new EscapeCode('r', "\r")
        };

        public readonly char Code;
        public readonly string Value;

        public EscapeCode(char code, string value)
        {
            Code = code;
            Value = value;
        }

        private static string GetEscapeCodeValue(char escapeCode, char terminator)
        {
            if (escapeCode == terminator)
            {
                return terminator.ToString();
            }

            try
            {
                return Sequences.Single(seq => seq.Code == escapeCode).Value;
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException($"Unrecognised escape code \"{escapeCode}\"");
            }
        }

        public static string Unescape(string value, char terminator)
        {
            return EscapeCodeRegex.Replace(value, match => {
                return GetEscapeCodeValue(match.Groups[1].Captures[0].Value[0], terminator);
            });
        }

        public static string Escape(string value, char terminator)
        {
            var result = new StringBuilder(value);
            foreach (var sequence in Sequences)
            {
                result.Replace(sequence.Value, "\\" + sequence.Code);
            }
            result.Replace(terminator.ToString(), "\\" + terminator.ToString());

            return result.ToString();
        }
    }
}
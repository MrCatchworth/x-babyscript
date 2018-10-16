using Antlr4.Runtime;
using System;
using System.Collections.Generic;

namespace XRebirthBabyScript
{
    public class BabyScriptLogger
    {
        public void Write(string value)
        {
            Console.Out.Write(value);
        }
        
        public void WriteLine(string value)
        {
            Console.Out.WriteLine(value);
        }

        public void WriteError(string value)
        {
            Console.Error.Write(value);
        }

        public void WriteLineError(string value)
        {
            Console.Error.WriteLine(value);
        }

        public void LogError(string message)
        {
            WriteLineError(message);
        }

        public void LogError(string path, string message)
        {
            WriteLineError($"{path}: {message}");
        }

        public void LogError(string path, int line, string message)
        {
            WriteLineError($"{path}:{line}: {message}");
        }

        public void LogError(string path, int line, int column, string message)
        {
            WriteLineError($"{path}:{line}:{column}: {message}");
        }
    }
}
using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRebirthBabyScript.Compile
{
    public class SemanticError
    {
        public SemanticError(string path, IToken offendingToken, string message)
        {
            Path = path;
            OffendingToken = offendingToken;
            Message = message;
        }

        public string Path { get; set; }
        public IToken OffendingToken { get; set; }
        public string Message { get; set; }
    }
}

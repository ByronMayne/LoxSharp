using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage
{
    public abstract class LoxExceptions : Exception
    {
        public LoxExceptions() : base()
        {

        }

        public LoxExceptions(string message) : base(message)
        {

        }
    }

    public class ParserException : LoxExceptions
    {
    }

    public class BreakException : LoxExceptions
    {
    }

    public class RuntimeError : LoxExceptions
    {
        public Token token { get; protected set; }

        public RuntimeError(Token token, string message) : base(message)
        {
            this.token = token;
        }
    }
}

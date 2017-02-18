using System.Diagnostics;

namespace LoxLanguage
{
    [DebuggerDisplay("{type} Token: {line} {lexeme}")]
    public struct Token
    {
        public TokenType type;
        public string lexeme;
        public object literal;
        public int line;

        public Token(TokenType type, string lexeme, object literal, int line)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }

        /// <summary>
        /// A handy string name for this class.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if(literal != null)
            {
                return string.Format("{0} {1} {2}", type, lexeme, literal);
            }
            else
            {
                return string.Format("{0} {1}", type, lexeme);
            }
        }
    }
}

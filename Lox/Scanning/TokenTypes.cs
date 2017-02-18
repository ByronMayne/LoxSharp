namespace LoxLanguage
{
    /// <summary>
    /// An enum that contains all our lexemes.
    /// </summary>
    public enum TokenType
    {
        // No token defined
        Undefined,

        // Single-character tokens.
       LeftParen,
       RightParen,
       LeftBrace,
       RightBrace,

       // One or two character tokens
       Bang,
       BangEqual,
       Equal,
       EqualEqual,
       Greater,
       GreaterEqual,
       Less,
       LessEqual,

       // Literals
       Identifier,
       String, 
       Number,

       // Others
       Comma,
       Dot,
       Minus,
       Plus,
       Semicolon,
       Star,
       Slash,

       // Keywords
       And,
       Class,
       Else,
       False,
       Fun,
       For,
       If,
       Nil,
       Or,
       Print,
       Return,
       Super,
       This,
       True,
       Var,
       While,


       EOF
    }
}

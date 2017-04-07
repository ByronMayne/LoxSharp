namespace LoxLanguage
{
    /// <summary>
    /// An enum that contains all our lexemes.
    /// </summary>
    public enum TokenType
    {
        // No token defined
        Undefined,

       /// <summary>
       /// {
       /// </summary>
       LeftParen,
       /// <summary>
       /// }
       /// </summary>
       RightParen,
       /// <summary>
       /// [
       /// </summary>
       LeftBrace,
       /// <summary>
       /// ]
       /// </summary>
       RightBrace,

       /// <summary>
       /// [
       /// </summary>
       LeftBracket,

       /// <summary>
       /// ]
       /// </summary>
       RightBracket,

       // One or two character tokens
       /// <summary>
       /// !
       /// </summary>
        Bang,
       /// <summary>
       /// !=
       /// </summary>
       BangEqual,
       /// <summary>
       /// =
       /// </summary>
       Equal,
       /// <summary>
       /// ==
       /// </summary>
       EqualEqual,
       /// <summary>
       /// >
       /// </summary>
       Greater,
       /// <summary>
       /// >=
       /// </summary>
       GreaterEqual,
       /// <summary>
       /// <
       /// </summary>
       Less,
       /// <summary>
       /// <= 
       /// </summary>
       LessEqual,
       /// <summary>
       /// ++
       /// </summary>
       PlusPlus,
       /// <summary>
       /// --
       /// </summary>
       MinusMinus,
       /// <summary>
       /// ?
       /// </summary>
       Question,
       /// <summary>
       /// :
       /// </summary>
       Colon,

       /// <summary>
       /// %
       /// </summary>
       Modulus,

       // Literals
       Identifier,
       String, 
       Number,

       // Others
       /// <summary>
       /// ,
       /// </summary>
       Comma,
       /// <summary>
       /// .
       /// </summary>
       Dot,
       /// <summary>
       /// -
       /// </summary>
       Minus,
       /// <summary>
       /// +
       /// </summary>
       Plus,
       /// <summary>
       /// ;
       /// </summary>
       Semicolon,
       /// <summary>
       /// *
       /// </summary>
       Star,
       /// <summary>
       /// /
       /// </summary>
       Slash,

       // Keywords
       /// <summary>
       /// And
       /// </summary>
       And,
       /// <summary>
       /// class
       /// </summary>
       Class,
       /// <summary>
       /// else
       /// </summary>
       Else,
       /// <summary>
       /// false
       /// </summary>
       False,
       /// <summary>
       /// fun
       /// </summary>
       Fun,
       /// <summary>
       /// for
       /// </summary>
       For,
       /// <summary>
       /// if
       /// </summary>
       If,
       /// <summary>
       /// nil
       /// </summary>
       Nil,
       /// <summary>
       /// or
       /// </summary>
       Or,
       /// <summary>
       /// print
       /// </summary>
       Print,
       /// <summary>
       /// return
       /// </summary>
       Return,
       /// <summary>
       /// super
       /// </summary>
       Super,
       /// <summary>
       /// this
       /// </summary>
       This,
       /// <summary>
       /// true
       /// </summary>
       True,
       /// <summary>
       /// var
       /// </summary>
       Var,
       /// <summary>
       /// while
       /// </summary>
       While,


       EOF
    }
}

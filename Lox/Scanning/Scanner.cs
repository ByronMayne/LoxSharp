using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LoxLanguage
{
    public class Scanner
    {
        private List<Token> m_Tokens;
        private string m_Source;
        private int m_Start;
        private int m_Current;
        private int m_Line;
        private IErrorHandler m_ErrorHandler;

        public Scanner(string source, IErrorHandler errorHandler)
        {
            m_Tokens = new List<Token>();
            m_Source = source;
            m_Start = 0;
            m_Line = 0;
            m_Current = 0;
            m_ErrorHandler = errorHandler;
        }

        /// <summary>
        /// Returns true if we are at the end of a file.
        /// </summary>
        public bool isAtEnd
        {
            get { return m_Current >= m_Source.Length; }
        }

        public List<Token> ScanTokens()
        {
            // Create new token list
            m_Tokens = new List<Token>();
            // Loop until we are out of chars. 
            while (!isAtEnd)
            {
                m_Start = m_Current;
                ScanToken();
            }
            // Add the end of file token
            m_Tokens.Add(new Token(TokenType.EOF, "", null, m_Line));
            // Return our result. 
            return m_Tokens;
        }

        /// <summary>
        /// Takes our current string and checks for the next token.
        /// </summary>
        private void ScanToken()
        {
            // Get our next char
            char nextChar = Read();
            // Switch on it
            switch (nextChar)
            {
                // Braces
                case '(': AddToken(TokenType.LeftParen); break;
                case ')': AddToken(TokenType.RightParen); break;
                case '{': AddToken(TokenType.LeftBrace); break;
                case '}': AddToken(TokenType.RightBrace); break;
                // Syntax
                case ',': AddToken(TokenType.Comma); break;
                case '.': AddToken(TokenType.Dot); break;
                // Math
                case '-': AddToken(TokenType.Minus); break;
                case '+': AddToken(TokenType.Plus); break;
                case ';': AddToken(TokenType.Semicolon); break;
                case '*': AddToken(TokenType.Star); break;
                case '!': AddToken(Match('=') ? TokenType.BangEqual : TokenType.Bang); break;
                case '=': AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal); break;
                case '<': AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less); break;
                case '>': AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater); break;
                // Long Literals
                case '"': ParseStringToken(); break;

                // Whitespace
                case ' ':
                case '\r':
                case '\t':
                    break;
                case '\n':
                    m_Line++;
                    break;

                case '/':
                    if (Match('/'))
                    {
                        // A comment goes to the end of a line
                        while (Peek() != '\n' && !isAtEnd)
                        {
                            // Move to the next char
                            Read();
                        }
                    }
                    else if(Match('*'))
                    {
                        ParseMulitLineComment();
                    }
                    else
                    {
                        AddToken(TokenType.Slash);
                    }
                    break;
                default:
                    // Handle all numbers
                    if (char.IsDigit(nextChar))
                    {
                        ParseNumberToken();
                    }
                    else if (IsAlpha(nextChar))
                    {
                        ParseIdentifier();
                    }
                    else
                    {
                        m_ErrorHandler.Error(m_Line, string.Format("Unexpected character '{0}'", nextChar));
                    }
                    break;
            }
        }

        /// <summary>
        /// Starts parsing out a token from a string
        /// </summary>
        private void ParseStringToken()
        {
            while (Peek() != '"' && !isAtEnd)
            {
                // We allow multi line comments
                if (Peek() == '\n')
                {
                    m_Line++;
                }
                Read();
            }

            // Unterminated string.
            if (isAtEnd)
            {
                m_ErrorHandler.Error(m_Line, "Unterminated string");
                return;
            }

            // Skip to next line
            Read();

            // Trim the surrounding quotes
            string value = Substring(m_Start + 1, m_Current - 1);
            AddToken(TokenType.String, value);
        }

        /// <summary>
        /// Starts parsing out a number token from a string
        /// </summary>
        public void ParseNumberToken()
        {
            // First part of number
            while (char.IsDigit(Peek()))
            {
                Read();
            }

            // Look for a fractional part.
            if (Peek() == '.' && char.IsDigit(PeekNext()))
            {
                Read();

                // Last part of number.
                while (char.IsDigit(Peek()))
                {
                    Read();
                }
            }

            // Parse our string
            string asString = Substring(m_Start, m_Current);
            double asDouble = double.Parse(asString);
            // Add our token
            AddToken(TokenType.Number, asDouble);
        }

        /// <summary>
        /// Starts parsing out a identifier token from a string
        /// </summary>
        private void ParseIdentifier()
        {
            while (IsAlphaNumeric(Peek()))
            {
                Read();
            }

            // Grab our text
            string text = Substring(m_Start, m_Current);

            // Create our token
            TokenType type = Keywords.Get(text);
            // Check if it's not null
            if (type == TokenType.Undefined)
            {
                type = TokenType.Identifier;
            }
            AddToken(type);
        }

        /// <summary>
        /// Reads  text in a in our source until we reach the end
        /// of a multi line comment
        /// </summary>
        private void ParseMulitLineComment()
        {
            int depth = 1;
            int startingLine = m_Line;
            while(true)
            {
                if (isAtEnd)
                {
                    m_ErrorHandler.Error(startingLine, "Unterminated multi line comment");
                    break;
                }

                char current = Read();

                if(current == '/' && Match('*'))
                {
                    // We hit a nested comment
                    depth++;
                }
                else if (current == '*' && Match('/'))
                {
                    depth--;

                    if (depth == 0)
                    {
                        break;
                    }
                }

                if(current == '\n')
                {
                    m_Line++;
                }
            }
        }

        /// <summary>
        /// Returns if this char is a letter or an underscore.
        /// </summary>
        public bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   c == '_';
        }

        /// <summary>
        /// Returns true if the character is a digit or a letter.
        /// </summary>
        public bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || char.IsDigit(c);
        }

        /// <summary>
        /// Checks to see if our next char is the expected one if it is the char is consumed
        /// if not it's left alone.
        /// </summary>
        private bool Match(char expected)
        {
            // We can't scan past the end
            if (isAtEnd)
            {
                return false;
            }
            // Is our expected char there?
            if (m_Source[m_Current] != expected)
            {
                return false;
            }
            m_Current++;
            return true;
        }

        /// <summary>
        /// Advances to the next Char
        /// </summary>
        /// <returns></returns>
        private char Read()
        {
            m_Current++;
            return m_Source[m_Current - 1];
        }

        /// <summary>
        /// Returns the current char without consuming it.
        /// </summary>
        public char Peek()
        {
            if (m_Current >= m_Source.Length)
            {
                return '\0';
            }
            return m_Source[m_Current];
        }

        /// <summary>
        /// Returns the next char without consuming it.
        /// </summary>
        public char PeekNext()
        {
            if (m_Current + 1 >= m_Source.Length)
            {
                return '\0';
            }
            return m_Source[m_Current + 1];
        }

        /// <summary>
        /// Creates a substring of our source from a start
        /// and ending position.
        /// </summary>
        private string Substring(int start, int end)
        {
            int length = end - start;
            return m_Source.Substring(start, length);
        }

        /// <summary>
        /// Adds a new null token to our list. 
        /// </summary>
        private void AddToken(TokenType tokenType)
        {
            AddToken(tokenType, null);
        }

        /// <summary>
        /// Adds a new token to our list with values.
        /// </summary>
        private void AddToken(TokenType tokenType, object literal)
        {
            string text = Substring(m_Start, m_Current);
            m_Tokens.Add(new Token(tokenType, text, literal, m_Line));
        }
    }
}

using LoxLanguage;
using System.Collections.Generic;
using System;

namespace LoxLanguage
{
    public class Parser
    {
        private IList<Token> m_Tokens;
        private int m_Current;
        private IErrorHandler m_ErrorHandler;

        /// <summary>
        /// Creates a new instance of our parser.
        /// </summary>
        /// <param name="tokens"></param>
        public Parser(IList<Token> tokens, IErrorHandler errorHanlder)
        {
            m_ErrorHandler = errorHanlder;
            m_Tokens = tokens;
            m_Current = 0;
        }

        private Expression Expression()
        {
            return Equality();
        }

        public Expression Equality()
        {
            Expression expression = Comparison();

            while (Match(TokenType.BangEqual, TokenType.EqualEqual))
            {
                Token @operator = Previous();
                Expression right = Comparison();
                expression = new Expression.Binary(expression, @operator, right);
            }

            return expression;
        }

        /// <summary>
        /// It checks to see if the current token is any of the given types. If so, it
        /// consumes it and returns true. Otherwise, it returns false and leaves the token where it is.
        /// </summary>
        private bool Match(params TokenType[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                if (Check(types[i]))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This returns true if the current token is of the given type. Unlike match(), it doesn’t consume the 
        /// token, it only looks at it.
        /// </summary>
        private bool Check(TokenType tokenType)
        {
            if (IsAtEnd())
            {
                return false;
            }
            return Peek().type == tokenType;
        }


        /// <summary>
        /// This consumes the current token and returns it, similar to
        /// how our scanner’s advance() method did with characters.
        /// </summary>
        private Token Advance()
        {
            if (!IsAtEnd())
            {
                m_Current++;
            }
            return Previous();
        }

        /// <summary>
        /// Returns true if we are on the last token or end of file and
        /// false if we are not. 
        /// </summary>
        private bool IsAtEnd()
        {
            return m_Tokens[m_Current].type == TokenType.EOF;
        }

        /// <summary>
        /// returns the current token we have yet to consume.
        /// </summary>
        private Token Peek()
        {
            return m_Tokens[m_Current];
        }


        /// <summary>
        /// returns the most recently consumed token
        /// </summary>
        private Token Previous()
        {
            return m_Tokens[m_Current - 1];
        }

        /// <summary>
        /// The grammar rule is virtually identical and so is the code.
        /// The only differences are the token types for the operators we match, and the 
        /// method we call for the operands, now term(). The remaining two binary
        /// operator rules follow the same pattern:
        /// </summary>
        /// <returns></returns>
        private Expression Comparison()
        {
            Expression expression = Term();

            while (Match(TokenType.Greater, TokenType.GreaterEqual, TokenType.LeftBrace, TokenType.LessEqual))
            {
                Token @operator = Previous();
                Expression right = Term();
                expression = new Expression.Binary(expression, @operator, right);
            }

            return expression;
        }

        private Expression Term()
        {
            Expression expression = Factor();

            while (Match(TokenType.Minus, TokenType.Plus))
            {
                Token @operator = Previous();
                Expression right = Factor();
                expression = new Expression.Binary(expression, @operator, right);
            }

            return expression;
        }

        private Expression Factor()
        {
            Expression expression = Unary();

            while (Match(TokenType.Slash, TokenType.Star))
            {
                Token @operator = Previous();
                Expression right = Unary();
                expression = new Expression.Binary(expression, @operator, right);
            }

            return expression;
        }

        private Expression Unary()
        {
            if (Match(TokenType.Bang, TokenType.Minus))
            {
                Token @operator = Previous();
                Expression right = Unary();
                return new Expression.Unary(@operator, right);
            }
            return Primary();
        }

        private Expression Primary()
        {
            if (Match(TokenType.False)) return new Expression.Literal(false);
            if (Match(TokenType.True)) return new Expression.Literal(true);
            if (Match(TokenType.Nil)) return new Expression.Literal(null);

            if (Match(TokenType.Number, TokenType.String))
            {
                return new Expression.Literal(Previous().literal);
            }

            if (Match(TokenType.LeftParen))
            {
                Expression expression = Expression();
                Consume(TokenType.RightParen, "Expect ')' after expression.");
                return new Expression.Grouping(expression);
            }

            return null;
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type))
            {
                return Advance();
            }
            throw Error(Peek(), message);
        }

        private ParserException Error(Token token, string message)
        {
            m_ErrorHandler.Error(token, message);
            return new ParserException();
        }

        private void Synchronize()
        {
            Advance();

            while(!IsAtEnd())
            {
                if(Previous().type == TokenType.Semicolon)
                {
                    return;
                }

                switch(Peek().type)
                {
                    case TokenType.Class:
                    case TokenType.Fun:
                    case TokenType.Var:
                    case TokenType.For:
                    case TokenType.If:
                    case TokenType.While:
                    case TokenType.Print:
                    case TokenType.Return:
                        return;
                }

                Advance();
            }
        }
    }
}

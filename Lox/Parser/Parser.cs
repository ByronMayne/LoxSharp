using LoxLanguage;
using System.Collections.Generic;
using System;

namespace LoxLanguage
{
    public class Parser
    {
        private const int FUNCTION_MAX_ARG_COUNT = 32;
        private IList<Token> m_Tokens;
        private int m_Current;
        private int m_LoopDepth = 0;
        private IErrorHandler m_ErrorHandler;

        /// <summary>
        /// Creates a new instance of our parser. The <see cref="IErrorHandler"/> was
        /// added by me to handle errors.
        /// </summary>
        /// <param name="tokens"></param>
        public Parser(IList<Token> tokens, IErrorHandler errorHanlder)
        {
            m_ErrorHandler = errorHanlder;
            m_Tokens = tokens;
            m_Current = 0;
        }

        /// <summary>
        /// Starts the parsing processes for our list of tokens. 
        /// </summary>
        /// <returns></returns>
        public List<Stmt> Parse()
        {
            List<Stmt> statements = new List<Stmt>();
            while (!IsAtEnd())
            {
                statements.Add(Declaration());
            }
            return statements;
        }


        private Stmt Declaration()
        {
            try
            {
                if (Match(TokenType.Var)) return VarDeclartion();

                return Statement();
            }
            catch (ParserException e)
            {
                Synchronize();
                return null;
            }
        }

        private Stmt VarDeclartion()
        {
            Token name = Consume(TokenType.Identifier, "Expect variable name.");

            Expr initializer = null;
            if (Match(TokenType.Equal))
            {
                initializer = Expression();
            }

            Consume(TokenType.Semicolon, "Expect ';' after variable declaration.");
            return new Stmt.Var(name, initializer);
        }

        private Stmt Statement()
        {
            if (Match(TokenType.For)) return ForStatement();
            if (Match(TokenType.Break)) return BreakStatement();
            if (Match(TokenType.If)) return IfStatement();
            if (Match(TokenType.Print)) return PrintStatement();
            if (Match(TokenType.While)) return WhileStatement();
            if (Match(TokenType.LeftBrace)) return new Stmt.Block(Block());

            return ExpressionStatement();
        }

        private Expr Expression()
        {
            return Assignment();
        }

        private Expr Assignment()
        {
            Expr expr = Or();

            if (Match(TokenType.Equal))
            {
                Token equal = Previous();
                Expr value = Assignment();

                if (expr is Expr.Variable)
                {
                    Token name = ((Expr.Variable)expr).name;
                    return new Expr.Assign(name, value);
                }
                Error(equal, "Invalid assignment target.");
            }
            return expr;
        }

        private Expr Or()
        {
            Expr expr = And();

            while (Match(TokenType.Or))
            {
                Token @operator = Previous();
                Expr right = And();
                expr = new Expr.Logical(expr, @operator, right);
            }
            return expr;
        }

        private Expr And()
        {
            Expr expr = Equality();
            while (Match(TokenType.And))
            {
                Token @operator = Previous();
                Expr right = Equality();
                expr = new Expr.Logical(expr, @operator, right);
            }
            return expr;
        }

        private Stmt PrintStatement()
        {
            Expr value = Expression();
            Consume(TokenType.Semicolon, "Expect ';' after value.");
            return new Stmt.Print(value);
        }

        private Stmt ForStatement()
        {
            Consume(TokenType.LeftParen, "Expect '(' after 'for'.");

            Stmt initializer;
            if (Match(TokenType.Semicolon))
            {
                // None is defined
                initializer = null;
            }
            else if (Match(TokenType.Var))
            {
                initializer = VarDeclartion();
            }
            else
            {
                initializer = ExpressionStatement();
            }

            Expr condition = null;
            if (!Check(TokenType.Semicolon))
            {
                condition = Expression();
            }
            Consume(TokenType.Semicolon, "Expect ';' after loop condition");

            Expr increment = null;
            if (!Check(TokenType.RightParen))
            {
                increment = Expression();
            }
            Consume(TokenType.RightParen, "Expect ')' after for clauses.");

            Exception exception = null;
            try
            {
                m_LoopDepth++;
                Stmt body = Statement();

                if (increment != null)
                {
                    Stmt.Expression expression = new Stmt.Expression(increment);
                    List<Stmt> content = new List<Stmt>() { body, expression };
                    body = new Stmt.Block(content);
                }

                if (condition == null)
                {
                    condition = new Expr.Literal(true);
                }

                body = new Stmt.While(condition, body);

                if (initializer != null)
                {
                    List<Stmt> content = new List<Stmt>() { initializer, body };
                    body = new Stmt.Block(content);
                }

                return body;
            }
            catch (Exception e)
            {
                exception = e;
            }
            finally
            {
                m_LoopDepth--;
            }

            throw exception;
        }

        public Stmt BreakStatement()
        {
            if (m_LoopDepth == 0)
            {
                Error(Previous(), "Must be inside a loop to use 'break'");
            }
            Consume(TokenType.Semicolon, "Expect ';' after 'break'.");
            return new Stmt.Break();
        }

        private Stmt IfStatement()
        {
            Consume(TokenType.LeftParen, "Expect '(' after 'if'.");
            Expr condition = Expression();
            Consume(TokenType.RightParen, "Expect ')' after if condition.");

            Stmt thenBranch = Statement();
            Stmt elseBranch = null;
            if (Match(TokenType.Else))
            {
                elseBranch = Statement();
            }
            return new Stmt.If(condition, thenBranch, elseBranch);
        }


        private Stmt WhileStatement()
        {
            Consume(TokenType.LeftParen, "Expect '(' after 'while'.");
            Expr condition = Expression();
            Consume(TokenType.RightParen, "Expect ')' after condition.");
            Exception exception = null;
            try
            {
                m_LoopDepth++;
                Stmt body = Statement();
                return new Stmt.While(condition, body);
            }
            catch (Exception e)
            {
                exception = e;
            }
            finally
            {
                m_LoopDepth--;
            }
            throw exception;
        }

        private List<Stmt> Block()
        {
            List<Stmt> statements = new List<Stmt>();

            while (!Check(TokenType.RightBrace) && !IsAtEnd())
            {
                statements.Add(Declaration());
            }

            Consume(TokenType.RightBrace, "Expect '}' after block.");

            return statements;
        }

        public Stmt ExpressionStatement()
        {
            Expr expr = Expression();
            Consume(TokenType.Semicolon, "Expect ':' after expression.");
            return new Stmt.Expression(expr);
        }

        private Expr Conditional()
        {
            Expr expression = Equality();

            if (Match(TokenType.Question))
            {
                Expr thenBranch = Expression();
                Consume(TokenType.Colon, "Expect ':' after then branch of conditional expression.");
                Expr elseBranch = Conditional();
                expression = new Expr.Conditional(expression, thenBranch, elseBranch);
            }
            return expression;
        }

        public Expr Equality()
        {
            Expr expression = Comparison();

            while (Match(TokenType.BangEqual, TokenType.EqualEqual))
            {
                Token @operator = Previous();
                Expr right = Comparison();
                expression = new Expr.Binary(expression, @operator, right);
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
        private Expr Comparison()
        {
            Expr expression = Term();

            while (Match(TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual))
            {
                Token @operator = Previous();
                Expr right = Term();
                expression = new Expr.Binary(expression, @operator, right);
            }

            return expression;
        }

        private Expr Term()
        {
            Expr expression = Factor();

            while (Match(TokenType.Minus, TokenType.Plus))
            {
                Token @operator = Previous();
                Expr right = Factor();
                expression = new Expr.Binary(expression, @operator, right);
            }

            return expression;
        }

        private Expr Factor()
        {
            Expr expression = Unary();

            while (Match(TokenType.Slash, TokenType.Star, TokenType.Modulus))
            {
                Token @operator = Previous();
                Expr right = Unary();
                expression = new Expr.Binary(expression, @operator, right);
            }

            return expression;
        }

        private Expr Unary()
        {
            if (Match(TokenType.Bang, TokenType.Minus, TokenType.MinusMinus, TokenType.PlusPlus))
            {
                Token @operator = Previous();
                Expr right = Unary();
                return new Expr.Prefix(@operator, right);
            }
            return Call();
        }

        private Expr Call()
        {
            Expr expr = Postfix();

            while(true)
            {
                if(Match(TokenType.LeftParen))
                {
                    expr = FinishCall(expr);
                }
                else
                {
                    break;
                }
            }

            return expr;
        }

        private Expr Postfix()
        {
            Expr expression = Primary();

            while (Match(TokenType.MinusMinus, TokenType.PlusPlus))
            {
                expression = new Expr.Postfix(Previous(), expression);
            }
            return expression;
        }

        private Expr Primary()
        {
            if (Match(TokenType.False)) return new Expr.Literal(false);
            if (Match(TokenType.True)) return new Expr.Literal(true);
            if (Match(TokenType.Nil)) return new Expr.Literal(null);

            if (Match(TokenType.Number, TokenType.String))
            {
                return new Expr.Literal(Previous().literal);
            }

            if (Match(TokenType.Identifier))
            {
                return new Expr.Variable(Previous());
            }

            if (Match(TokenType.LeftParen))
            {
                Expr expression = Expression();
                Consume(TokenType.RightParen, "Expect ')' after expression.");
                return new Expr.Grouping(expression);
            }

            // Error productions
            if (Match(TokenType.BangEqual, TokenType.EqualEqual))
            {
                Error(Previous(), "Missing left-hand operand.");
                Equality();
                return null;
            }

            if (Match(TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual))
            {
                Error(Previous(), "Missing left-hand operand.");
                Comparison();
                return null;
            }

            if (Match(TokenType.Plus))
            {
                Error(Previous(), "Missing left-hand operand.");
                Term();
                return null;
            }

            if (Match(TokenType.Slash, TokenType.Star, TokenType.Modulus))
            {
                Error(Previous(), "Missing left-hand operand.");
                Factor();
                return null;
            }

            throw Error(Peek(), "Expected expression.");
        }
        
        private Expr FinishCall(Expr callee)
        {
            List<Expr> arguments = new List<Expr>();
            if(!Check(TokenType.RightParen))
            {
                do
                {
                    arguments.Add(Expression());

                    if (arguments.Count > FUNCTION_MAX_ARG_COUNT)
                    {
                        Error(Peek(), "Cannon have more then " + FUNCTION_MAX_ARG_COUNT + " arguments for a function");
                    }
                }
                while (Match(TokenType.Comma));
            }

            Token paren = Consume(TokenType.RightParen, "Expect ')' after arguments.");

            return new Expr.Call(callee, paren, arguments);
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

            while (!IsAtEnd())
            {
                if (Previous().type == TokenType.Semicolon)
                {
                    return;
                }

                switch (Peek().type)
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

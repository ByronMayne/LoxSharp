using System;
using System.Collections.Generic;

namespace LoxLanguage
{
    public class Interpreter : Expr.IVisitor<object>, Stmt.IVisitor<object>
    {
        private IErrorHandler m_ErrorHandler;
        private Environment m_Enviroment;

        public Interpreter(IErrorHandler errorHandler)
        {
            m_Enviroment = new Environment();
            m_ErrorHandler = errorHandler;
            DefineNativeFunctions();
        }

        private void DefineNativeFunctions()
        {
            ILoxCallable clock = new NativeFunction_Clock();
        }

        public void Interpret(List<Stmt> statements)
        {
            try
            {
                for (int i = 0; i < statements.Count; i++)
                {
                    Execute(statements[i]);
                }
            }
            catch (RuntimeError error)
            {
                m_ErrorHandler.RuntimeError(error);
            }
        }

        public object Visit(Expr.Literal literal)
        {
            return literal.value;
        }

        public object Visit(Expr.Postfix postfix)
        {
            object right = Evaluate(postfix.lhs);
            ValidateNumberOperand(postfix.opp, right);
            switch (postfix.opp.type)
            {
                case TokenType.MinusMinus:
                    right = (double)right - 1;
                    break;
                case TokenType.PlusPlus:
                    right = (double)right + 1;
                    break;
            }
            /// This was made up by me because in the book there is nothing that assigns this value. 
            // Cast to variable type
            Expr.Variable asVariable = (Expr.Variable)postfix.lhs;
            // Assign it's value
            m_Enviroment.Assign(asVariable.name, right);
            // Return the value. 
            return right;
        }

        public object Visit(Expr.Conditional conditional)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Prefix prefix)
        {
            object right = Evaluate(prefix.rhs);

            switch (prefix.opp.type)
            {
                case TokenType.Bang:
                    return !IsTrue(right);
                case TokenType.Minus:
                    ValidateNumberOperand(prefix.opp, right);
                    return -(double)right;
                case TokenType.MinusMinus:
                    ValidateNumberOperand(prefix.opp, right);
                    return (double)right - 1;
                case TokenType.PlusPlus:
                    ValidateNumberOperand(prefix.opp, right);
                    return (double)right + 1;
            }

            // Unreachable
            return null;
        }



        public object Visit(Expr.Grouping grouping)
        {
            return Evaluate(grouping.expression);
        }

        public object Visit(Expr.Binary binary)
        {
            object right = Evaluate(binary.rhs);
            object left = Evaluate(binary.lhs);


            switch (binary.opp.type)
            {
                case TokenType.Greater:
                    ValidateNumberOperand(binary.opp, left, right);
                    return (double)left > (double)right;
                case TokenType.GreaterEqual:
                    ValidateNumberOperand(binary.opp, left, right);
                    return (double)left >= (double)right;
                case TokenType.Less:
                    ValidateNumberOperand(binary.opp, left, right);
                    return (double)left < (double)right;
                case TokenType.LessEqual:
                    ValidateNumberOperand(binary.opp, left, right);
                    return (double)left <= (double)right;
                case TokenType.Minus:
                    ValidateNumberOperand(binary.opp, left, right);
                    return (double)left - (double)right;
                case TokenType.Plus:
                    if (left is double && right is double)
                    {
                        return (double)left + (double)right;
                    }
                    else if (left is string)
                    {
                        return (string)left + Stringify(right);
                    }
                    // Not valid addition type. 
                    throw new RuntimeError(binary.opp, "Operands must be two numbers or two strings.");
                case TokenType.Modulus:
                    ValidateNumberOperand(binary.opp, left, right);
                    return (double)left % (double)right;
                case TokenType.Slash:
                    ValidateNumberOperand(binary.opp, left, right);
                    ValidateDivision(binary.opp, right);
                    return (double)left / (double)right;
                case TokenType.Star:
                    ValidateNumberOperand(binary.opp, left, right);
                    return (double)left * (double)right;
                case TokenType.BangEqual:
                    return !IsEqual(left, right);
                case TokenType.EqualEqual:
                    return IsEqual(left, right);

            }

            // Unreachable. 
            return null;
        }

        /// <summary>
        /// Checks if a value is true or not in Lox. Null is false and a boolean
        /// returns it's value. Everything else is true. 
        /// </summary>
        private bool IsTrue(object value)
        {
            if (value == null) return false;
            if (value is bool) return (bool)value;
            return true;
        }

        /// <summary>
        /// Takes two objects and checks if they are equal.
        /// </summary>
        private bool IsEqual(object a, object b)
        {
            return Equals(a, b);
        }

        /// <summary>
        /// Returns the value of the expression.
        /// </summary>
        private object Evaluate(Expr expression)
        {
            return expression.Accept(this);
        }

        /// <summary>
        /// Accepts the incoming visitor to this class.
        /// </summary>
        /// <param name="stmt"></param>
        private object Execute(Stmt stmt)
        {
            if (stmt != null)
            {
                return stmt.Accept(this);
            }
            return null;
        }

        /// <summary>
        /// Loops over all the statements in a block and sets their local environment. 
        /// </summary>
        /// <param name="statements">The list of statements that we want to run in our block.</param>
        /// <param name="environment">The environment we store their local variables in.</param>
        private void ExecuteBlock(List<Stmt> statements, Environment environment)
        {
            Environment previous = m_Enviroment;
            try
            {
                m_Enviroment = environment;

                for (int i = 0; i < statements.Count; i++)
                {
                    Execute(statements[i]);
                }
            }
            finally
            {
                m_Enviroment = previous;
            }
        }

        /// <summary>
        /// Validates that a token and a value are both a number
        /// operand;
        /// </summary>
        private void ValidateNumberOperand(Token operand, object right)
        {
            if (right is double) return;
            throw new RuntimeError(operand, "Operand must be a number");
        }

        /// <summary>
        /// Validates that a token and a value are both a number
        /// operand;
        /// </summary>
        private void ValidateNumberOperand(Token operand, object right, object left)
        {
            if (right is double && left is double) return;
            throw new RuntimeError(operand, "Operands must be a number");
        }

        /// <summary>
        /// Checks to see if we are dividing by zero and if we are it throws
        /// and exception. 
        /// </summary>
        private void ValidateDivision(Token operand, object value)
        {
            if ((double)value == 0)
            {
                throw new RuntimeError(operand, "Divided by zero");
            }
        }

        /// <summary>
        /// Converts any value into a displayable string. 
        /// </summary>
        private string Stringify(object value)
        {
            if (value == null) return "nil";

            return value.ToString();
        }

        public object Visit(Expr.Assign _assign)
        {
            object value = Evaluate(_assign.value);
            m_Enviroment.Assign(_assign.name, value);
            return value;
        }

        public object Visit(Expr.Call _call)
        {
            object callee = Evaluate(_call.callee);
            object[] arguments = new object[_call.arguments.Count];
            for(int i = 0; i < _call.arguments.Count; i++)
            {
                arguments[i] = Evaluate(_call.arguments[i]);
            }

            ILoxCallable function = callee as ILoxCallable;

            if(callee == null)
            {
                throw new RuntimeError(_call.paren, "Can only call functions on classes");
            }

            if (arguments.Length != function.arity)
            {
                throw new RuntimeError(_call.paren, "Expected " + function.arity + " arguments but got " + arguments.Length + ".");
            }

            return function.Call(this, arguments);
        }

        public object Visit(Expr.Get _get)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Logical _logical)
        {
            object left = Evaluate(_logical.left);

            if (_logical.opp.type == TokenType.Or)
            {
                if (IsTrue(left)) return left;
            }
            else
            {
                if (!IsTrue(left)) return left;
            }

            return Evaluate(_logical.right);
        }

        public object Visit(Expr.Set _set)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Super _super)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.This _this)
        {
            throw new NotImplementedException();
        }

        public object Visit(Stmt.Block _block)
        {
            Environment blockScope = new Environment(m_Enviroment);
            ExecuteBlock(_block.statements, blockScope);
            return null;
        }

        public object Visit(Stmt.Class _class)
        {
            throw new NotImplementedException();
        }

        public object Visit(Stmt.Expression _expression)
        {
            Evaluate(_expression.expression);
            return null;
        }

        public object Visit(Stmt.Function _function)
        {
            throw new NotImplementedException();
        }

        public object Visit(Stmt.If _if)
        {
            if (IsTrue(Evaluate(_if.condition)))
            {
                Execute(_if.thenBranch);
            }
            else if (_if.elseBranch != null)
            {
                Execute(_if.elseBranch);
            }
            return null;
        }

        public object Visit(Stmt.Print _print)
        {
            object value = Evaluate(_print.expression);
            string output = Stringify(value);
            Console.WriteLine(output);
            return null;
        }

        public object Visit(Stmt.Return _return)
        {
            throw new NotImplementedException();
        }

        public object Visit(Stmt.Var _var)
        {
            object value = null;
            if (_var.initializer != null)
            {
                value = Evaluate(_var.initializer);
            }

            m_Enviroment.Define(_var.name.lexeme, value);
            return null;
        }

        public object Visit(Stmt.While _while)
        {
            try
            {
                while (IsTrue(Evaluate(_while.condition)))
                {
                    Execute(_while.body);
                }
            }
            catch (BreakException)
            {
                // Do nothing 
            }
            return null;
        }

        public object Visit(Expr.Variable _variable)
        {
            return m_Enviroment.Get(_variable.name);
        }

        public object Visit(Stmt.Break _break)
        {
            throw new BreakException();
        }
    }
}

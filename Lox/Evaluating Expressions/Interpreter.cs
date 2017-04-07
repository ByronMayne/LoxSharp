using System;
using static LoxLanguage.Expression;

namespace LoxLanguage
{
    public class Interpreter : IVisitor<object>
    {
        private IErrorHandler m_ErrorHandler; 

        public Interpreter(IErrorHandler errorHandler)
        {
            m_ErrorHandler = errorHandler;
        }

        public void Interpret(Expression expresssion)
        {
            try
            {
                object value = Evaluate(expresssion);
                Console.WriteLine(Stringify(value));
            }
            catch( RuntimeError error)
            {
                m_ErrorHandler.RuntimeError(error);
            }
        }



        public object Visit(Literal literal)
        {
            return literal.value;
        }

        public object Visit(Postfix postfix)
        {
            throw new NotImplementedException();
        }

        public object Visit(Conditional conditional)
        {
            throw new NotImplementedException();
        }

        public object Visit(Prefix prefix)
        {
            object right = Evaluate(prefix.rhs);

            switch(prefix.opp.type)
            {
                case TokenType.Bang:
                    return !IsTrue(right);
                case TokenType.Minus:
                    ValidateNumberOperand(prefix.opp, right); 
                    return -(double)right;
            }

            // Unreachable
            return null;
        }



        public object Visit(Grouping grouping)
        {
            return Evaluate(grouping.expression);
        }

        public object Visit(Binary binary)
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
                    else if (left is string && right is string)
                    {
                        return (string)left + (string)right;
                    }
                    // Not valid addition type. 
                    return new RuntimeError(binary.opp, "Operands must be two numbers or two strings.");
                case TokenType.Slash:
                    ValidateNumberOperand(binary.opp, left, right);
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
        private object Evaluate(Expression expression)
        {
            return expression.Accept(this);
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
        /// Converts any value into a displayable string. 
        /// </summary>
        private string Stringify(object value)
        {
            if (value == null) return "nil";

            return value.ToString();
        }
    }
}

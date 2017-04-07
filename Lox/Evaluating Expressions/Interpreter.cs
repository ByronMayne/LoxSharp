using System;
using static LoxLanguage.Expression;

namespace LoxLanguage
{
    public class Interpreter : IVisitor<object>
    {
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
                    return (double)left > (double)right;
                case TokenType.GreaterEqual:
                    return (double)left >= (double)right;
                case TokenType.Less:
                    return (double)left < (double)right;
                case TokenType.LessEqual:
                    return (double)left <= (double)right;
                case TokenType.Minus:
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
                    break;
                case TokenType.Slash:
                    return (double)left / (double)right;
                case TokenType.Star:
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
        private bool IsEqual(object left, object right)
        {

        }

        /// <summary>
        /// Returns the value of the expression.
        /// </summary>
        private object Evaluate(Expression expression)
        {
            return expression.Accept(this);
        }
    }
}

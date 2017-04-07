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
            throw new NotImplementedException();
        }

        public object Visit(Grouping grouping)
        {
            return Evaluate(grouping.expression);
        }

        public object Visit(Binary binary)
        {
            throw new NotImplementedException();
        }

        private object Evaluate(Expression expression)
        {
            return expression.Accept(this);
        }
    }
}

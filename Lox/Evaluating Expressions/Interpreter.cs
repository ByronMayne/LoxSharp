using System;
using static LoxLanguage.Expression;

namespace LoxLanguage
{
    public class Interpreter : IVisitor<object>
    {
        public object Visit(Literal literal)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public object Visit(Binary binary)
        {
            throw new NotImplementedException();
        }
    }
}

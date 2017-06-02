using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage
{
    public class ExpressionPrinter : Expression.IVisitor<string>
    {
        public string Print(Expression expression)
        {
            return expression.Accept(this);
        }

        public string Visit(Expression.Get _get)
        {
            throw new NotImplementedException();
        }

        public string Visit(Expression.Super _super)
        {
            throw new NotImplementedException();
        }

        public string Visit(Expression.Set _set)
        {
            throw new NotImplementedException();
        }

        public string Visit(Expression.Logical _logical)
        {
            throw new NotImplementedException();
        }

        public string Visit(Expression.Call _call)
        {
            throw new NotImplementedException();
        }

        public string Visit(Expression.This _this)
        {
            throw new NotImplementedException();
        }

        public string Visit(Expression.Assign _assign)
        {
            throw new NotImplementedException();
        }

        private string Parenthesize(string name, params Expression[] Expression)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(").Append(name);
            foreach(Expression expr in Expression)
            {
                builder.Append(" ");
                builder.Append(expr.Accept(this));
            }
            builder.Append(")");
            return builder.ToString();
        }

        // Visitors
        string Expression.IVisitor<string>.Visit(Expression.Binary binary)
        {
            return Parenthesize(binary.opp.lexeme, binary.lhs, binary.rhs);
        }

        string Expression.IVisitor<string>.Visit(Expression.Grouping grouping)
        {
            return Parenthesize("Group", grouping.expression);
        }

        string Expression.IVisitor<string>.Visit(Expression.Literal literal)
        {
            return literal.value.ToString();
        }

        string Expression.IVisitor<string>.Visit(Expression.Prefix prefix)
        {
            return Parenthesize(prefix.opp.lexeme, prefix.rhs);
        }

        string Expression.IVisitor<string>.Visit(Expression.Postfix postfix)
        {
            return Parenthesize(postfix.opp.lexeme, postfix.lhs);
        }
        string Expression.IVisitor<string>.Visit(Expression.Conditional conditional)
        {
            return Parenthesize("Conditional", conditional.thenBranch, conditional.elseBranch);
        }
    }
}

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
    }
}

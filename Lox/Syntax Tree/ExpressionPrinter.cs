using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage
{
    public class ExpressionPrinter : Expressions.Visitor<string>
    {
        public string Print(Expressions expression)
        {
            return expression.Accept(this);
        }


        private string Parenthesize(string name, params Expressions[] expressions)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(").Append(name);
            foreach(Expressions expr in expressions)
            {
                builder.Append(" ");
                builder.Append(expr.Accept(this));
            }
            builder.Append(")");
            return builder.ToString();
        }

        // Visitors
        string Expressions.Visitor<string>.Visit(Expressions.Binary binary)
        {
            return Parenthesize(binary.opp.lexeme, binary.lhs, binary.rhs);
        }

        string Expressions.Visitor<string>.Visit(Expressions.Grouping grouping)
        {
            return Parenthesize("Group", grouping.expression);
        }

        string Expressions.Visitor<string>.Visit(Expressions.Literal literal)
        {
            return literal.value.ToString();
        }

        string Expressions.Visitor<string>.Visit(Expressions.Unary unary)
        {
            return Parenthesize(unary.opp.lexeme, unary.rhs);
        }
    }
}

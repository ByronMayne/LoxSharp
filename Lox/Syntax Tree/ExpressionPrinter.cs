using System;
using System.Text;

namespace LoxLanguage
{
    public class ExpressionPrinter : Expr.IVisitor<string>
    {
        public string Print(Expr expression)
        {
            return expression.Accept(this);
        }

        public string Visit(Expr.Get _get)
        {
            throw new NotImplementedException();
        }

        public string Visit(Expr.Super _super)
        {
            throw new NotImplementedException();
        }

        public string Visit(Expr.Set _set)
        {
            throw new NotImplementedException();
        }

        public string Visit(Expr.Logical _logical)
        {
            throw new NotImplementedException();
        }

        public string Visit(Expr.Call _call)
        {
            throw new NotImplementedException();
        }

        public string Visit(Expr.This _this)
        {
            throw new NotImplementedException();
        }

        public string Visit(Expr.Assign _assign)
        {
            throw new NotImplementedException();
        }

        private string Parenthesize(string name, params Expr[] Expression)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(").Append(name);
            foreach(Expr expr in Expression)
            {
                builder.Append(" ");
                builder.Append(expr.Accept(this));
            }
            builder.Append(")");
            return builder.ToString();
        }

        // Visitors
        string Expr.IVisitor<string>.Visit(Expr.Binary binary)
        {
            return Parenthesize(binary.opp.lexeme, binary.lhs, binary.rhs);
        }

        string Expr.IVisitor<string>.Visit(Expr.Grouping grouping)
        {
            return Parenthesize("Group", grouping.expression);
        }

        string Expr.IVisitor<string>.Visit(Expr.Literal literal)
        {
            return literal.value.ToString();
        }

        string Expr.IVisitor<string>.Visit(Expr.Prefix prefix)
        {
            return Parenthesize(prefix.opp.lexeme, prefix.rhs);
        }

        string Expr.IVisitor<string>.Visit(Expr.Postfix postfix)
        {
            return Parenthesize(postfix.opp.lexeme, postfix.lhs);
        }
        string Expr.IVisitor<string>.Visit(Expr.Conditional conditional)
        {
            return Parenthesize("Conditional", conditional.thenBranch, conditional.elseBranch);
        }
    }
}

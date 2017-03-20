 
namespace LoxLanguage
{
	public abstract class Expression
	{
		public interface IVisitor<T> 
		{
            T Visit(Binary binary);
            T Visit(Grouping grouping);
            T Visit(Literal literal);
            T Visit(Unary unary);
		}
 
        public class Binary : Expression
        {
            public Expression lhs;
            public Token opp;
            public Expression rhs;
             
            public Binary(Expression lhs, Token opp, Expression rhs)
            {
                this.lhs = lhs;
                this.opp = opp;
                this.rhs = rhs;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Grouping : Expression
        {
            public Expression expression;
             
            public Grouping(Expression expression)
            {
                this.expression = expression;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Literal : Expression
        {
            public object value;
             
            public Literal(object value)
            {
                this.value = value;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Unary : Expression
        {
            public Token opp;
            public Expression rhs;
             
            public Unary(Token opp, Expression rhs)
            {
                this.opp = opp;
                this.rhs = rhs;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

		/// <summary>
		/// Base function for visiting our trees.
		/// </summary> 
		public abstract T Accept<T>(IVisitor<T> visitor);
	}
}



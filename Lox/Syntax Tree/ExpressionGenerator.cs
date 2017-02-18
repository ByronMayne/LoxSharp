 
namespace LoxLanguage
{
	public abstract class Expressions
	{
		public interface Visitor<T> 
		{
            T Visit(Binary binary);
            T Visit(Grouping grouping);
            T Visit(Literal literal);
            T Visit(Unary unary);
		}
 
        public class Binary : Expressions
        {
            public Expressions lhs;
            public Token opp;
            public Expressions rhs;
             
            public Binary(Expressions lhs, Token opp, Expressions rhs)
            {
                this.lhs = lhs;
                this.opp = opp;
                this.rhs = rhs;
            }
             
            public override T Accept<T>(Visitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Grouping : Expressions
        {
            public Expressions expression;
             
            public Grouping(Expressions expression)
            {
                this.expression = expression;
            }
             
            public override T Accept<T>(Visitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Literal : Expressions
        {
            public object value;
             
            public Literal(object value)
            {
                this.value = value;
            }
             
            public override T Accept<T>(Visitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Unary : Expressions
        {
            public Token opp;
            public Expressions rhs;
             
            public Unary(Token opp, Expressions rhs)
            {
                this.opp = opp;
                this.rhs = rhs;
            }
             
            public override T Accept<T>(Visitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

		/// <summary>
		/// Base function for visiting our trees.
		/// </summary> 
		public abstract T Accept<T>(Visitor<T> visitor);
	}
}



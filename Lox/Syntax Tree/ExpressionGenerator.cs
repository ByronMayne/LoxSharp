 
using System.Collections.Generic;
namespace LoxLanguage
{
	public abstract class Expression
	{
		public interface IVisitor<T> 
		{
            T Visit(Assign _assign);
            T Visit(Binary _binary);
            T Visit(Call _call);
            T Visit(Get _get);
            T Visit(Grouping _grouping);
            T Visit(Literal _literal);
            T Visit(Logical _logical);
            T Visit(Set _set);
            T Visit(Super _super);
            T Visit(This _this);
            T Visit(Prefix _prefix);
            T Visit(Postfix _postfix);
            T Visit(Conditional _conditional);
		}
 
        public class Assign : Expression
        {
            public Token name;
            public Expression value;
             
            public Assign(Token name, Expression value)
            {
                this.name = name;
                this.value = value;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
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

        public class Call : Expression
        {
            public Expression callee;
            public Token paren;
            public List<Expression> arguments;
             
            public Call(Expression callee, Token paren, List<Expression> arguments)
            {
                this.callee = callee;
                this.paren = paren;
                this.arguments = arguments;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Get : Expression
        {
            public Expression target;
            public Token name;
             
            public Get(Expression target, Token name)
            {
                this.target = target;
                this.name = name;
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

        public class Logical : Expression
        {
            public Expression left;
            public Token opp;
            public Expression right;
             
            public Logical(Expression left, Token opp, Expression right)
            {
                this.left = left;
                this.opp = opp;
                this.right = right;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Set : Expression
        {
            public Expression target;
            public Token name;
            public Expression value;
             
            public Set(Expression target, Token name, Expression value)
            {
                this.target = target;
                this.name = name;
                this.value = value;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Super : Expression
        {
            public Token keyword;
            public Token method;
             
            public Super(Token keyword, Token method)
            {
                this.keyword = keyword;
                this.method = method;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class This : Expression
        {
            public Token keyword;
             
            public This(Token keyword)
            {
                this.keyword = keyword;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Prefix : Expression
        {
            public Token opp;
            public Expression rhs;
             
            public Prefix(Token opp, Expression rhs)
            {
                this.opp = opp;
                this.rhs = rhs;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Postfix : Expression
        {
            public Token opp;
            public Expression lhs;
             
            public Postfix(Token opp, Expression lhs)
            {
                this.opp = opp;
                this.lhs = lhs;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Conditional : Expression
        {
            public Expression expression;
            public Expression thenBranch;
            public Expression elseBranch;
             
            public Conditional(Expression expression, Expression thenBranch, Expression elseBranch)
            {
                this.expression = expression;
                this.thenBranch = thenBranch;
                this.elseBranch = elseBranch;
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



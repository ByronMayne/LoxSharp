 
using System.Collections.Generic;
namespace LoxLanguage
{
	public abstract class Statement
	{
		public interface IVisitor<T> 
		{
            T Visit(Block _block);
            T Visit(Class _class);
            T Visit(Expression _expression);
            T Visit(Function _function);
            T Visit(If _if);
            T Visit(Print _print);
            T Visit(Return _return);
            T Visit(Var _var);
            T Visit(While _while);
		}
 
        public class Block : Statement
        {
            public List<Statement> statements;
             
            public Block(List<Statement> statements)
            {
                this.statements = statements;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Class : Statement
        {
            public Token name;
            public Expression superClass;
            public List<Statement.Function> methods;
             
            public Class(Token name, Expression superClass, List<Statement.Function> methods)
            {
                this.name = name;
                this.superClass = superClass;
                this.methods = methods;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Expression : Statement
        {
            public Expression expression;
             
            public Expression(Expression expression)
            {
                this.expression = expression;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Function : Statement
        {
            public Token name;
            public List<Token> parameters;
            public List<Statement> body;
             
            public Function(Token name, List<Token> parameters, List<Statement> body)
            {
                this.name = name;
                this.parameters = parameters;
                this.body = body;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class If : Statement
        {
            public Token condition;
            public Statement thenBranch;
            public Statement elseBranch;
             
            public If(Token condition, Statement thenBranch, Statement elseBranch)
            {
                this.condition = condition;
                this.thenBranch = thenBranch;
                this.elseBranch = elseBranch;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Print : Statement
        {
            public Statement expression;
             
            public Print(Statement expression)
            {
                this.expression = expression;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Return : Statement
        {
            public Token keyword;
            public Expression value;
             
            public Return(Token keyword, Expression value)
            {
                this.keyword = keyword;
                this.value = value;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Var : Statement
        {
            public Token name;
            public Expression initializer;
             
            public Var(Token name, Expression initializer)
            {
                this.name = name;
                this.initializer = initializer;
            }
             
            public override T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class While : Statement
        {
            public Expression condition;
            public Statement body;
             
            public While(Expression condition, Statement body)
            {
                this.condition = condition;
                this.body = body;
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



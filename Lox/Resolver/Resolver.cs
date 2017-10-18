using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LoxLanguage
{
    public class Scope : Dictionary<string, bool>
    {
    }

    public class ScopeStack : List<Scope>
    {
        public Scope Peek()
        {
            return this[0];
        }


        public void Pop()
        {
            RemoveAt(0);
        }

        public void Push(Scope scope)
        {
            Insert(0, scope);
        }
    }

    public class Resolver : Expr.IVisitor<object>, Stmt.IVisitor<object>
    {
        private Interpreter m_Iterpreter;
        private ScopeStack m_Scopes;
        private IErrorHandler m_ErrorHandler;

        public Resolver(Interpreter interpreter, IErrorHandler errorHandler)
        {
            m_Iterpreter = interpreter;
            m_ErrorHandler = errorHandler;
            m_Scopes = new ScopeStack();
        }

        private void BeginScope()
        {
            Scope scope = new Scope();
            m_Scopes.Push(scope);
        }

        private void EndScope()
        {
            m_Scopes.Pop();
        }

        private void Resolve(List<Stmt> statements)
        {
            for (int i = 0; i < statements.Count; i++)
            {
                Resolve(statements[i]);
            }
        }

        private void Resolve(Expr expr)
        {
            expr.Accept(this);
        }

        private void Resolve(Stmt stmt)
        {
            stmt.Accept(this);
        }

        private void ResolveLocal(Expr _variable, Token name)
        {
            for (int i = m_Scopes.Count; i >= 0; i--)
            {
                Scope scope = m_Scopes[i];
                if (scope.ContainsKey(name.lexeme))
                {
                    m_Iterpreter.Resolve(_variable, m_Scopes.Count - 1 - i);
                    return;
                }
            }
        }

        private void ResolveFunction(Stmt.Function stmt, TokenType fun)
        {
            BeginScope();
            {
                foreach(Token paramter in stmt.parameters)
                {
                    Declare(paramter);
                    Define(paramter);
                }
                Resolve(stmt.body);
            }
            EndScope();
        }

        private void Define(Token name)
        {
            if (m_Scopes.Count == 0)
            {
                return;
            }
            Scope scope = m_Scopes.Peek();
            scope[name.lexeme] = true;
        }

        private void Declare(Token name)
        {
            if (m_Scopes.Count == 0)
            {
                return;
            }
            Scope scope = m_Scopes.Peek();
            scope[name.lexeme] = false;
        }

        public object Visit(Expr.Call _call)
        {
            Resolve(_call.callee);
            foreach(Expr argument in _call.arguments)
            {
                Resolve(argument); 
            }
            return null;
        }

        public object Visit(Expr.Grouping _grouping)
        {
            Resolve(_grouping.expression);
            return null;
        }

        public object Visit(Expr.Logical _logical)
        {
            Resolve(_logical.right); 
            return null;
        }

        public object Visit(Expr.Super _super)
        {
            return null;
        }

        public object Visit(Expr.Prefix _prefix)
        {
            return null;
        }

        public object Visit(Expr.Conditional _conditional)
        {
            return null;
        }

        public object Visit(Stmt.Block _block)
        {
            BeginScope();
            Resolve(_block.statements);
            EndScope();
            return null;
        }

        public object Visit(Stmt.Expression _expression)
        {
            Resolve(_expression.expression);
            return null;
        }

        public object Visit(Stmt.If _if)
        {
            Resolve(_if.condition);
            Resolve(_if.thenBranch);
            if(_if.elseBranch != null)
            {
                Resolve(_if.elseBranch); 
            }
            return null;
        }

        public object Visit(Stmt.Return _return)
        {
            if(_return.value != null)
            {
                Resolve(_return.value);
            }
            return null;
        }

        public object Visit(Stmt.While _while)
        {
            Resolve(_while.condition);
            Resolve(_while.body);
            return null;
        }

        public object Visit(Stmt.Break _break)
        {
            return null;
        }

        public object Visit(Stmt.Var stmt)
        {
            Declare(stmt.name);
            if (stmt.initializer != null)
            {
                Resolve(stmt.initializer);
            }
            Define(stmt.name);
            return null;
        }

        public object Visit(Stmt.Print _print)
        {
            Resolve(_print.expression);
            return null;
        }

        public object Visit(Stmt.Function stmt)
        {
            Declare(stmt.name);
            Define(stmt.name);
            ResolveFunction(stmt, TokenType.Fun);
            return null;
        }



        public object Visit(Stmt.Class _class)
        {
            return null;
        }

        public object Visit(Expr.Variable _variable)
        {
            if (m_Scopes.Count == 0)
            {
                m_ErrorHandler.Error(_variable.name, "Cannot read local variable in its own initializer.");
            }

            Scope scope = m_Scopes.Peek();

            if (!scope.ContainsKey(_variable.name.lexeme) || scope[_variable.name.lexeme] == false)
            {
                m_ErrorHandler.Error(_variable.name, "Cannot read local variable in its own initializer.");
            }

            ResolveLocal(_variable, _variable.name);
            return null;
        }

        public object Visit(Expr.Postfix _postfix)
        {
            return null;
        }

        public object Visit(Expr.This _this)
        {
            return null;
        }

        public object Visit(Expr.Set _set)
        {
            return null;
        }

        public object Visit(Expr.Literal _literal)
        {
            return null;
        }

        public object Visit(Expr.Get _get)
        {
            return null;
        }

        public object Visit(Expr.Binary _binary)
        {
            Resolve(_binary.lhs);
            Resolve(_binary.rhs);
            return null;
        }

        public object Visit(Expr.Assign _assign)
        {
            Resolve(_assign.value);
            ResolveLocal(_assign, _assign.name);
            return null;
        }
    }
}

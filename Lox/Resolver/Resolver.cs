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
            for(int i = 0; i < statements.Count; i++)
            {
                Resolve(statements[i]);
            }
        }

        private void Resolve(Stmt stmt)
        {
            stmt.Accept(this);
        }

        private void Resolve(Expr initializer)
        {
            throw new NotImplementedException();
        }

        private void Define(Token name)
        {
            if(m_Scopes.Count == 0)
            {
                return;
            }
            Scope scope = m_Scopes.Peek();
            scope[name.lexeme] = true;
        }

        private void Declare(Token name)
        {
            if(m_Scopes.Count == 0)
            {
                return;
            }
            Scope scope = m_Scopes.Peek();
            scope[name.lexeme] = false; 
        }

        public object Visit(Expr.Call _call)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Grouping _grouping)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Logical _logical)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Super _super)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Prefix _prefix)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Conditional _conditional)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public object Visit(Stmt.If _if)
        {
            throw new NotImplementedException();
        }

        public object Visit(Stmt.Return _return)
        {
            throw new NotImplementedException();
        }

        public object Visit(Stmt.While _while)
        {
            throw new NotImplementedException();
        }

        public object Visit(Stmt.Break _break)
        {
            throw new NotImplementedException();
        }

        public object Visit(Stmt.Var _var)
        {
            Declare(_var.name);
            if(_var.initializer != null)
            {
                Resolve(_var.initializer);
            }
            Define(_var.name);
            return null;
        }



        public object Visit(Stmt.Print _print)
        {
            throw new NotImplementedException();
        }

        public object Visit(Stmt.Function _function)
        {
            throw new NotImplementedException();
        }

        public object Visit(Stmt.Class _class)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Variable _variable)
        {
            if(m_Scopes.Count == 0)
            {
                m_ErrorHandler.Error(_variable.name, "Cannot read local variable in its own initializer.");
            }

            Scope scope = m_Scopes.Peek();

            if(!scope.ContainsKey(_variable.name.lexeme) || scope[_variable.name.lexeme] == false)
            {
                m_ErrorHandler.Error(_variable.name, "Cannot read local variable in its own initializer.");
            }

            ResolveLocal(_variable, _variable.name);
            return null;
        }

        private void ResolveLocal(Expr.Variable _variable, Token name)
        {
            for(int i = m_Scopes.Count; i  >= 0; i--)
            {
                Scope scope = m_Scopes[i];
                if(scope.ContainsKey(name.lexeme))
                {
                    m_Iterpreter.Resolve(_variable, m_Scopes.Count - 1 - i);
                    return;
                }
            }
        }

        public object Visit(Expr.Postfix _postfix)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.This _this)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Set _set)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Literal _literal)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Get _get)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Binary _binary)
        {
            throw new NotImplementedException();
        }

        public object Visit(Expr.Assign _assign)
        {
            throw new NotImplementedException();
        }
    }
}

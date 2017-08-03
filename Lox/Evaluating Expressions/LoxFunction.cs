using LoxLanguage.Exceptions;
using System;
using System.Collections.Generic;

namespace LoxLanguage
{
    public class LoxFunction : ILoxCallable
    {
        private Stmt.Function m_Declaration;
        private Environment m_Closure;

        public int arity
        {
            get
            {
                return m_Declaration.parameters.Count;
            }
        }

        public object Call(Interpreter interpreter, IList<object> arguements)
        {
            Environment environment = new Environment(m_Closure);
            for (int i = 0; i < m_Declaration.parameters.Count; i++)
            {
                environment.Define(m_Declaration.parameters[i].lexeme, arguements[i]);
            }
            try
            {
                interpreter.ExecuteBlock(m_Declaration.body, environment);
            }
            catch(Return returnValue)
            {
                return returnValue.value;
            }
            return null;
        }

        public LoxFunction(Stmt.Function declaration, Environment closure)
        {
            m_Declaration = declaration;
            m_Closure = closure;
        }

        public override string ToString()
        {
            return string.Format("<fn {0}>", m_Declaration.name.lexeme);
        }
    }
}

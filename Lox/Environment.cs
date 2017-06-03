using System;
using System.Collections.Generic;

namespace LoxLanguage
{
    public class Environment
    {
        private Environment m_Enclosing;
        private Dictionary<string, object> m_Values;

        public Environment()
        {
            m_Values = new Dictionary<string, object>();
            m_Enclosing = null;
        }

        public Environment(Environment enclosing) : this()
        {
            m_Enclosing = enclosing;
        }

        public void Define(string name, object value)
        {
            m_Values[name] = value;
        }

        public object Get(Token name)
        {
            if(m_Values.ContainsKey(name.lexeme))
            {
                return m_Values[name.lexeme];
            }

            if (m_Enclosing != null) return m_Enclosing.Get(name); 

            throw new RuntimeError(name, "Undefined variable '" + name.lexeme + ".");
        }

        public void Assign(Token name, object value)
        {
            if(m_Values.ContainsKey(name.lexeme))
            {
                m_Values[name.lexeme] = value;
                return;
            }

            if(m_Enclosing != null)
            {
                m_Enclosing.Assign(name, value);
                return;
            }

            throw new RuntimeError(name, "Undefined variable '" + name.lexeme + "'.");
        }
    }
}

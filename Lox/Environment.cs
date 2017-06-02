using System;
using System.Collections.Generic;

namespace LoxLanguage
{
    public class Environment
    {
        private Dictionary<string, object> m_Values;

        public Environment()
        {
            m_Values = new Dictionary<string, object>();
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
            throw new RuntimeError(name, "Undefined variable '" + name.lexeme + ".");
        }
    }
}

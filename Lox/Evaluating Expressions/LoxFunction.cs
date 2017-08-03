using LoxLanguage.Exceptions;
using System;
using System.Collections.Generic;

namespace LoxLanguage
{
    public class LoxFunction : ILoxCallable
    {
        private Stmt.Function declaration;

        public int arity
        {
            get
            {
                return declaration.parameters.Count;
            }
        }

        public object Call(Interpreter interpreter, IList<object> arguements)
        {
            Environment environment = new Environment(interpreter.globals);
            for (int i = 0; i < declaration.parameters.Count; i++)
            {
                environment.Define(declaration.parameters[i].lexeme, arguements[i]);
            }
            try
            {
                interpreter.ExecuteBlock(declaration.body, environment);
            }
            catch(Return returnValue)
            {
                return returnValue.value;
            }
            return null;
        }

        public LoxFunction(Stmt.Function declaration)
        {
            this.declaration = declaration;
        }

        public override string ToString()
        {
            return string.Format("<fn {0}>", declaration.name.lexeme);
        }
    }
}

using System;
using System.Collections.Generic;

namespace LoxLanguage
{
    public class NativeFunction_Clock : ILoxCallable
    {
        public int arity
        {
            get { return 0; }
        }

        public object Call(Interpreter interpreter, IList<object> arguements)
        {
            return DateTime.Now.Second;
        }
    }
}

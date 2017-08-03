using System.Collections.Generic;

namespace LoxLanguage
{
    public interface ILoxCallable
    {
        int arity { get; }

        object Call(Interpreter interpreter, IList<object> arguements);
    }
}

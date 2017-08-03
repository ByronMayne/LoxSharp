using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage
{
    public interface ILoxCallable
    {
        int arity { get; }

        object Call(Interpreter interpreter, IList<object> arguements);
    }
}

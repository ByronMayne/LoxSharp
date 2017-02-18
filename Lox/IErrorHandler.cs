using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage
{
    public interface IErrorHandler
    {
        void Error(int line, string message);
    }
}

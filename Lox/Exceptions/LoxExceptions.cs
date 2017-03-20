using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage
{
    public abstract class LoxExceptions : Exception
    {
    }

    public class ParserException : LoxExceptions
    {
    }
}

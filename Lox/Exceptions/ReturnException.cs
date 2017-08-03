using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage.Exceptions
{
    public class Return : LoxExceptions
    {
        private object m_Value;

        public object value
        {
            get { return m_Value; }
        }

        public Return(object value) : base()
        {
            m_Value = value;
        }
    }
}

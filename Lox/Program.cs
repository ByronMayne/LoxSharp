using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage
{
    public class Program
    {
        /// <summary>
        /// The entry point for our application. 
        /// </summary>
        public static int Main(string[] args)
        {
            if (args.Length > 1)
            {
                Debug.LogError("Invalid arguments count. Usage: jlox [script]");
                return -1;
            }
            else
            {
                Lox lox = new Lox(args);
                int returnCode = lox.Run();
                return returnCode;
            }
        }
    }
}

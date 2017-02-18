using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage
{
    public static class Debug
    {
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }

        public static void Log(string message, params object[] args)
        {
            Log(string.Format(message, args));
        }

        public static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + message);
            Console.ResetColor();
        }

        public static void LogError(string message, params object[] args )
        {
            LogError(string.Format(message, args));
        }

        public static void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Warning: " + message);
            Console.ResetColor();
        }

        public static void LogWarning(string message, params object[] args)
        {
            LogWarning(string.Format(message, args));
        }
    }
}

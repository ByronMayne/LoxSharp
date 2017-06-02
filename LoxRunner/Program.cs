using LoxLanguage;
using System;

namespace LoxRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Lox lox = new Lox(new string[] { "D://Repositories//LoxSharp//LoxRunner//Example.Lox" });
            lox.Run();
            Console.ReadLine();
        }
    }
}

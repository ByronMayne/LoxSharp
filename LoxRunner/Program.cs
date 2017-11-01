using LoxLanguage;
using System;
using System.IO;

namespace LoxRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            string loxScript = Directory.GetCurrentDirectory() + "/../../Example.Lox";
            Lox lox = new Lox(new[] { loxScript });
            lox.Run();
            Console.ReadLine();
        }
    }
}

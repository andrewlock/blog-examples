using System;
using System.Threading;

namespace TestApp.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(30_000);
            Console.WriteLine("Hello World!");
        }
    }
}

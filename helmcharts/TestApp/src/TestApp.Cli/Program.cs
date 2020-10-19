using System;
using System.Threading;

namespace TestApp.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length > 0 && args[0] == "say-hello")
            {
                Console.WriteLine("Hello world!");
            }
            else
            {
                Console.WriteLine("Running migrations...");
                Thread.Sleep(30_000);
                Console.WriteLine("Mmigrations complete!");
            }
        }
    }
}

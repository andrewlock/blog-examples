using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {
        private static int _runCount = 0;
        private static int _lazyRunCount = 0;
        private static readonly ConcurrentDictionary<string, string> _dictionary
            = new ConcurrentDictionary<string, string>();
        private static readonly ConcurrentDictionary<string, Lazy<string>> _lazyDictionary
            = new ConcurrentDictionary<string, Lazy<string>>();

        public static void Main(string[] args)
        {
            //Run using the normal dictionary
            var task1 = Task.Run(() => PrintValue("The first value"));
            var task2 = Task.Run(() => PrintValue("The second value"));
            Task.WaitAll(task1, task2);

            //Run count: 2
            Console.WriteLine($"Run count: {_runCount}");

            //Run using the lazy dictionary
            var lazyTask1 = Task.Run(() => PrintValueLazy("The lazy first value"));
            var lazyTask2 = Task.Run(() => PrintValueLazy("The lazy second value"));
            Task.WaitAll(lazyTask1, lazyTask2);

            //Run count: 1
            Console.WriteLine($"Run count: {_lazyRunCount}");
        }

        public static void PrintValue(string valueToPrint)
        {
            var valueFound = _dictionary.GetOrAdd("key",
                        x =>
                        {
                            Interlocked.Increment(ref _runCount);
                            Thread.Sleep(100);
                            return valueToPrint;
                        });
            Console.WriteLine(valueFound);
        }

        public static void PrintValueLazy(string valueToPrint)
        {
            var valueFound = _lazyDictionary.GetOrAdd("key",
                        x => new Lazy<string>(
                            () =>
                                {
                                    Interlocked.Increment(ref _lazyRunCount);
                                    Thread.Sleep(100);
                                    return valueToPrint;
                                }));
            Console.WriteLine(valueFound.Value);
        }
    }
}

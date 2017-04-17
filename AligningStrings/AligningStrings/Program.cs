using System;

namespace AligningStrings
{
    class Program
    {
        readonly static decimal val1 = 1;
        readonly static decimal val2 = 12;
        readonly static decimal val3 = 1234.12m;
        readonly static long _long = 999_999_999_999;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            Console.WriteLine($"Number 1 {val1,10:C}");
            Console.WriteLine($"Number 2 {val2,10:C}");
            Console.WriteLine($"Number 3 {val3,10:C}");
            Console.WriteLine($"Number 3 {_long,10:C}");
            Console.WriteLine("");

            Console.WriteLine($"A small number {val1,10:C}");
            Console.WriteLine($"A bit bigger {val2,10:C}");
            Console.WriteLine($"A bit bigger again {val3,10:C}");
            Console.WriteLine("");

            var label1 = "A small number";
            var label2 = "A bit bigger";
            var label3 = "A bit bigger again";

            Console.WriteLine($"{label1,-18} {val1,10:C}");
            Console.WriteLine($"{label2,-18} {val2,10:C}");
            Console.WriteLine($"{label3,-18} {val3,10:C}");
            Console.WriteLine("");

            Console.WriteLine($"A small number {val1,10:C}");
            Console.WriteLine($"A big number   {_long,10:C}");
            Console.WriteLine("");

            var autoFormat = $"{1,10:C}";
            var explicitFormat = "     £1.00";
            Console.WriteLine("Values are equal = {0}", autoFormat == explicitFormat);

            //doesn't compile
            //var maxLength = Math.Max(label1.Length, label2.Length);
            //Console.WriteLine($"{label1,-maxLength} {val1,10:C}");
            //Console.WriteLine($"{label2,-maxLength} {val2,10:C}");
            var maxLength = Math.Max(label1.Length, label2.Length);
            Console.WriteLine($"{{0,-{maxLength}}} {{1,10:C}}", label1, val1);

        }
    }
}
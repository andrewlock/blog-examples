using System;
using System.Diagnostics;
using System.Reflection;
//[assembly: AssemblyVersion("1.2.3.4")]
//[assembly: AssemblyFileVersion("6.6.6.6")]
//[assembly: AssemblyInformationalVersion("So many numbers!")]

namespace VersionNumbers
{

    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();

            Console.WriteLine("This app's version numbers:");

            var assemblyVersion = assembly.GetName().Version;

            Console.WriteLine($"AssemblyVersion       {assemblyVersion}");

            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var fileVersion = fileVersionInfo.FileVersion;

            Console.WriteLine($"FileVersion           {fileVersion}");

            var informationVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            Console.WriteLine($"InformationalVersion  {informationVersion}");
        }
    }
}

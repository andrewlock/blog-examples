using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;


namespace AddingDefaultSecurityHeaders
{
    public static class Program{
        // Entry point for the application.
        public static void Main(string[] args)
        {
        var host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>()
            .Build();
        
        host.Run();
        }
    }
}
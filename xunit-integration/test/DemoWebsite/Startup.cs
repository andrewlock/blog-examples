using CustomMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;


namespace DemoWebsite
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseCustomHeaderMiddleware("X-Demo-Header", "Header custom value");
            app.UseMiddleware<EchoMiddleware>();
        }

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}

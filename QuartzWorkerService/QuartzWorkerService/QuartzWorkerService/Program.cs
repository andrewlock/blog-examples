using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddHostedService<Worker>();
                    services.AddQuartz(q =>
                    {
                        q.UseMicrosoftDependencyInjectionScopedJobFactory();

                        q.AddJobAndTrigger<HelloWorldJob>(hostContext.Configuration);

                        //var jobKey = new JobKey("HelloWorldJob");
                        //q.AddJob<HelloWorldJob>(opts => opts.WithIdentity(jobKey));
                        //q.AddTrigger(opts => opts
                        //    .ForJob(jobKey)
                        //    .WithIdentity("HelloWorldJob-trigger")
                        //    .WithCronSchedule("0/5 * * * * ?"));
                    });

                    services.AddQuartzHostedService(
                        q => q.WaitForJobsToComplete = true);
                });
    }
}

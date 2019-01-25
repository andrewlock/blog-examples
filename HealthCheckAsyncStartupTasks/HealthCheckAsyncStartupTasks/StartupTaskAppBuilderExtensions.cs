using Microsoft.AspNetCore.Builder;

namespace HealthCheckAsyncStartupTasks
{
    public static class StartupTaskAppBuilderExtensions
    {
        public static IApplicationBuilder UseStartupTasksMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<StartupTasksMiddleware>();
        }
    }
}
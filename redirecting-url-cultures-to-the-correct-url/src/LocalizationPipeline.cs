using Microsoft.AspNetCore.Builder;

namespace WebApplication
{
    public class LocalizationPipeline
    {
        public void Configure(IApplicationBuilder app, RequestLocalizationOptions options)
        {
            app.UseRequestLocalization(options);
            app.UseMiddleware<RedirectUnsupportedCulturesMiddleware>();
        }
    }
}
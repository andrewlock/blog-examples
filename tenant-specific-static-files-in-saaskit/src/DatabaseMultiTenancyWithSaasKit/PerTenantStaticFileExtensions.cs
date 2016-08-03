using System;
using DatabaseMultiTenancyWithSaasKit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

public static class PerTenantStaticFileExtensions
{
    /// <summary>
    /// Add per-tenant static files to your application.  
    /// </summary>
    /// <param name="app">The IApplicationBuilder instance  </param>
    /// <param name="pathPrefix">The prefix to look for in the route indicating tenant-specific static files e.g. 'tenantfile'</param>
    /// <param name="tenantFolderResolver">A function for obtaining the tenant specific sub folder in which files reside, for a given TTenant</param>
    /// <typeparam name="TTenant">The type of the tenant, as registered in Startup</typeparam>
    /// <returns></returns>
    public static IApplicationBuilder UsePerTenantStaticFiles<TTenant>(this IApplicationBuilder app, string pathPrefix, Func<TTenant, string> tenantFolderResolver)
    {
        var routeBuilder = new RouteBuilder(app);
        var routeTemplate = pathPrefix + "/{*filePath}";
        routeBuilder.MapRoute(routeTemplate, (IApplicationBuilder fork) =>
           {
               fork.UseMiddleware<TenantSpecificPathRewriteMiddleware<TTenant>>(pathPrefix, tenantFolderResolver);
               fork.UseStaticFiles();
           });
        var router = routeBuilder.Build();
        app.UseRouter(router);

        return app;
    }
}

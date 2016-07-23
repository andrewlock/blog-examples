using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SaasKit.Multitenancy;
using SaasKit.Multitenancy.Internal;

public static class PerTenantStaticFileExtensions
{
    public static IApplicationBuilder UsePerTenantStaticFiles<TTenant>(this IApplicationBuilder app, Func<TTenant, string> tenantFolderResolver)
    {
        return UsePerTenantStaticFiles<TTenant>(app, tenantFolderResolver, "/tenant");
    }

    public static IApplicationBuilder UsePerTenantStaticFiles<TTenant>(this IApplicationBuilder app, Func<TTenant, string> tenantFolderResolver, string prefix)
    {
        Ensure.Argument.NotNull(app, nameof(app));
        Ensure.Argument.NotNull(prefix, nameof(prefix));

        app.Use(nxt => new TenantPipelineMiddleware<TTenant>(
            nxt,
            app,
            (ctx, builder) =>
            {
                bool isTenantSpecificPath = false;
                var tenantTag = tenantFolderResolver.Invoke(ctx.Tenant);
                builder.Use(next =>
                {
                    return async httpContext =>
                    {
                        var originalPath = httpContext.Request.Path;
                        isTenantSpecificPath = originalPath.StartsWithSegments(new PathString(prefix));
                        if (isTenantSpecificPath)
                        {
                            //remove the prefix portion of the path
                            var pathRemainder = originalPath.ToUriComponent()
                            .Substring(prefix.Length);

                            var newPath = new PathString($"{prefix}/{tenantTag}{pathRemainder}");
                            httpContext.Request.Path = newPath;
                        }
                        await next(httpContext);

                        //replace the original url after the remaing middleware has finished processing
                        httpContext.Request.Path = originalPath;
                    };
                });

                //render static files
                builder.UseStaticFiles();

                //End pipeline if we had a tenant specific path, otherwise continue
                builder.Use(next =>
                {
                    return async httpContext =>
                    {
                        if (isTenantSpecificPath)
                        {
                            httpContext.Response.StatusCode = 404;
                        }
                        else
                        {
                            await next(httpContext);
                        }
                    };
                });
            }
            ).Invoke);
        return app;
    }
}

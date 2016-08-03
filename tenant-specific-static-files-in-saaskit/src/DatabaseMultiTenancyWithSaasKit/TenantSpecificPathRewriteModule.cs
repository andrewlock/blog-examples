using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using SaasKit.Multitenancy;

namespace DatabaseMultiTenancyWithSaasKit
{
    public class TenantSpecificPathRewriteMiddleware<TTenant>
    {
        private readonly RequestDelegate _next;
        private readonly string _pathPrefix;
        private readonly Func<TTenant, string> _tenantFolderResolver;
        private readonly ILogger _logger;

        public TenantSpecificPathRewriteMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            string pathPrefix,
            Func<TTenant, string> tenantFolderResolver
            )
        {
            Ensure.Argument.NotNull(next, nameof(next));
            Ensure.Argument.NotNull(loggerFactory, nameof(loggerFactory));
            Ensure.Argument.NotNull(pathPrefix, nameof(pathPrefix));
            Ensure.Argument.NotNull(tenantFolderResolver, nameof(tenantFolderResolver));

            _next = next;
            _pathPrefix = pathPrefix;
            _tenantFolderResolver = tenantFolderResolver;
            _logger = loggerFactory.CreateLogger<TenantSpecificPathRewriteMiddleware<TTenant>>();
        }

        public async Task Invoke(HttpContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            var tenantContext = context.GetTenantContext<TTenant>();

            if (tenantContext != null)
            {
                //remove the prefix portion of the path
                var originalPath = context.Request.Path;
                var tenantFolder = _tenantFolderResolver(tenantContext.Tenant);
                var filePath = context.GetRouteValue("filePath");

                var newPath = new PathString($"/{_pathPrefix}/{tenantFolder}/{filePath}");
                context.Request.Path = newPath;

                _logger.LogDebug($"Tenant specific static file requested, path rewritten from {originalPath} to {newPath} ");

                await _next(context);

                //replace the original url after the remaing middleware has finished processing
                context.Request.Path = originalPath;
            }
        }

    }
}
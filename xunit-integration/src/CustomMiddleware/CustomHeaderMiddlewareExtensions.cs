using System;
using Microsoft.AspNetCore.Builder;

namespace CustomMiddleware
{
    /// <summary>
    /// The <see cref="IApplicationBuilder"/> extensions for adding Security headers middleware support.
    /// </summary>
    public static class CustomHeadersMiddlewareExtensions
    {
        /// <summary>
        /// Adds middleware to your web application pipeline to automatically add security headers to requests
        /// </summary>
        /// <param name="app">The IApplicationBuilder passed to your Configure method</param>
        /// <param name="headerName">The name of a header to configure.</param>
        /// <param name="headerName">The value of a header to configure.</param>
        /// <returns>The original app parameter</returns>
        public static IApplicationBuilder UseCustomHeaderMiddleware(this IApplicationBuilder app, string headerName, string headerValue)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<CustomHeaderMiddleware>(headerName, headerValue);
        }

    }
}
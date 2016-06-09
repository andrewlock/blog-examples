using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CustomMiddleware
{
    /// <summary>
    /// An ASP.NET middleware for adding a simple custom header.
    /// </summary>
    public class CustomHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _headerName;
        private readonly string _headerValue;

        /// <summary>
        /// Instantiates a new <see cref="CustomHeadersMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="headerName">The name of the header to add</param>
        /// <param name="headerValue">The value of the header to add</param>
        public CustomHeaderMiddleware(RequestDelegate next, string headerName, string headerValue)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            _next = next;
            _headerName = headerName;
            _headerValue = headerValue;
        }

        /// <summary>
        /// Invoke the middleware
        /// </summary>
        /// <param name="context">The current context</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var response = context?.Response;
            if (response == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            
            response.Headers[_headerName] = _headerValue;

            await _next(context);
        }

    }
}

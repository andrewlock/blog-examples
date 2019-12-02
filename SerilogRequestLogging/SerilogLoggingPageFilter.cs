using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace SerilogRequestLogging
{
    public class SerilogLoggingPageFilter : IPageFilter
    {
        private readonly IDiagnosticContext _diagnosticContext;
        public SerilogLoggingPageFilter(IDiagnosticContext diagnosticContext)
        {
            _diagnosticContext = diagnosticContext;
        }
        
        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            var name = context.HandlerMethod?.Name ?? context.HandlerMethod?.MethodInfo.Name;
            if (name != null)
            {
                _diagnosticContext.Set("RazorPageHandler", name);
            }
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context){}
        public void OnPageHandlerExecuting(PageHandlerExecutingContext context) {}
    }
}
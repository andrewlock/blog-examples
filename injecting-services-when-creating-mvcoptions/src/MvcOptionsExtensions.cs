using System.Buffers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;

namespace injecting_services_when_creating_mvcoptions
{
    public static class MvcOptionsExtensions
    {
        public static void UseHtmlEncodeJsonInputFormatter(this MvcOptions opts, ILogger<MvcOptions> logger, ObjectPoolProvider objectPoolProvider)
        {
            opts.InputFormatters.RemoveType<JsonInputFormatter>();
    
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new HtmlEncodeContractResolver()
            };
    
            var jsonInputFormatter = new JsonInputFormatter(logger, serializerSettings, ArrayPool<char>.Shared, objectPoolProvider);
    
            opts.InputFormatters.Add(jsonInputFormatter);
        }
    }
}
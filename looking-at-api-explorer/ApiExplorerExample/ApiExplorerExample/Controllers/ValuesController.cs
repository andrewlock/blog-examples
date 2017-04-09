using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ApiExplorerExample.Controllers
{
    public class ValuesController : Controller
    {
        readonly IApiDescriptionGroupCollectionProvider _apiExplorer;
        public ValuesController(IApiDescriptionGroupCollectionProvider apiExplorer)
        {
            _apiExplorer = apiExplorer;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return View(_apiExplorer);
        }
    }
}

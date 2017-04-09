using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ApiExplorerExample.Controllers
{
    public class DocumentationController : Controller
    {
        readonly IApiDescriptionGroupCollectionProvider _apiExplorer;
        public DocumentationController(IApiDescriptionGroupCollectionProvider apiExplorer)
        {
            _apiExplorer = apiExplorer;
        }

        public IActionResult Index()
        {
            return View(_apiExplorer);
        }   
    }
}

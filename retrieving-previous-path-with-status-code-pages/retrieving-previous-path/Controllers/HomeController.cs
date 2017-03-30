using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace RetrievingPreviousPath.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Throw()
        {
            throw new Exception("Oh noes! It seems to be broken!");
        }

        public IActionResult Problem()
        {
            return StatusCode(500);
        }

        public IActionResult Error(int? statusCode = null)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            ViewData["ErrorUrl"] = feature?.OriginalPath;
            ViewData["ErrorQuerystring"] = (feature as StatusCodeReExecuteFeature)?.OriginalQueryString;

            if (statusCode.HasValue)
            {
                if (statusCode == 404 || statusCode == 500)
                {
                    return View(statusCode.ToString());
                }
            }
            return View();
        }
    }
}

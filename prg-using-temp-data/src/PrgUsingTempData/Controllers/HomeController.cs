using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrgUsingTempData.AttributeFilters;
using PrgUsingTempData.ViewModels;

namespace PrgUsingTempData.Controllers
{
    public class HomeController : Controller
    {
        [ImportModelState]
        public IActionResult Index()
        {
            return View(new EditModel());
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult Index(EditModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Success));
        }

        public IActionResult Success()
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

        public IActionResult Error()
        {
            return View();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddingLocalization.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AddingLocalization.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new HomeViewModel());
        }

        [HttpPost]
        public IActionResult Index(HomeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ViewData["Result"] = "Success!";
            return View(model);
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


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SessionStateAndGdpr2_1.Models;

namespace SessionStateAndGdpr2_1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            RecordInSession("Home");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            RecordInSession("About");
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            RecordInSession("Contact");
            return View();
        }

        private void RecordInSession(string action)
        {
            var paths = HttpContext.Session.GetString("actions") ?? string.Empty;
            HttpContext.Session.SetString("actions", paths + ";" + action);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UsingSessionState.ViewModels;

namespace UsingSessionState.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            const string sessionKey = "FirstSeen";
            DateTime dateFirstSeen;
            var value = HttpContext.Session.GetString(sessionKey);
            if (!string.IsNullOrEmpty(value))
            {
                dateFirstSeen = JsonConvert.DeserializeObject<DateTime>(value);
            }
            else
            {
                dateFirstSeen = DateTime.Now;
                var serialisedDate = JsonConvert.SerializeObject(dateFirstSeen);
                HttpContext.Session.SetString(sessionKey, serialisedDate);
            }

            var model = new SessionStateViewModel
            {
                DateSessionStarted = dateFirstSeen,
                Now = DateTime.Now
            };

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

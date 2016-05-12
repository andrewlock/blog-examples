using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ModelBindingASPNET4.Models;

namespace ModelBindingASPNET4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Person person)
        {
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndexApi(Person person)
        {
            return Json(person);
        }

        [HttpPost]
        public ActionResult IndexApiJsonHeader(Person person)
        {
            ValidateRequestHeader(Request);

            return Json(person);
        }

        private void ValidateRequestHeader(HttpRequestBase request)
        {
            string cookieToken = "";
            string formToken = "";

            var token = request.Headers["RequestVerificationToken"];
            if (!string.IsNullOrEmpty(token))
            {
                string[] tokens = token.Split(':');
                if (tokens.Length == 2)
                {
                    cookieToken = tokens[0].Trim();
                    formToken = tokens[1].Trim();
                }
            }
            AntiForgery.Validate(cookieToken, formToken);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
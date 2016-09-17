using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddingLocalization.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AddingLocalization.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
        }

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
            ViewData["Result"] = _localizer["Success!"];
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

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}

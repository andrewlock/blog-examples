using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RegisteringForDispose.Controllers
{
    public class HomeController : Controller
    {
        readonly Disposable _disposable;

        public HomeController(
            TransientCreatedByContainer transient,
            ScopedCreatedByFactory scoped,
            SingletonAddedManually manually,
            SingletonCreatedByContainer createByContainer)
        {
            _disposable = new RegisteredForDispose();
        }

        public IActionResult Index()
        {
            HttpContext.Response.RegisterForDispose(_disposable);
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

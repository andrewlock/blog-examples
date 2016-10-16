using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UiAuthorisation.Controllers
{
    public class VIPLoungeController : Controller
    {
        [Authorize("CanAccessVIPArea")]
        public IActionResult ViewTheFancySeatsInTheLounge()
        {
            return View();
        }
    }
}
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Route("{culture}/[controller]")]
    public class ValuesController : Controller
    {
        [Route("ShowMeTheCulture")]
        public string GetCulture()
        {
            return $"CurrentCulture:{CultureInfo.CurrentCulture.Name}, CurrentUICulture:{CultureInfo.CurrentUICulture.Name}";
        }
    }
}

using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class ValuesController : Controller
    {
        public string GetCulture()
        {
            return $"CurrentCulture:{CultureInfo.CurrentCulture.Name}, CurrentUICulture:{CultureInfo.CurrentUICulture.Name}";
        }
    }
}
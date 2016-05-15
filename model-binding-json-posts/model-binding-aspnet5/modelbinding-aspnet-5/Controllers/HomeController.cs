using Microsoft.AspNetCore.Mvc;
using ModelBindingASPNET5.Models;

namespace ModelBindingASPNET5
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index() => View();
        
        [HttpPost("/")]
        public IActionResult Index(Person person) => View(person);
        
        [HttpPost()]
        public IActionResult IndexApi(Person person){
            return Json(person);   
        } 
        
        [HttpPost()]
        public IActionResult IndexApiFromBody([FromBody]Person person)
        {
            return Json(person);
        }
        
        [HttpPost()]
        public IActionResult IndexApiFromForm([FromForm]Person person)
        {
            return Json(person);
        }
    }
}

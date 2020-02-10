using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AjaxRazorComponent.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty, Required] public string Greeting { get; set; }
        [BindProperty, Required] public string Country { get; set; }
        [BindProperty, Required] public string State { get; set; }
        public string Message { get; set; }

        public void OnGet() { }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                Message = $"{Greeting} from {State}, {Country}";
            }
        }
    }
}

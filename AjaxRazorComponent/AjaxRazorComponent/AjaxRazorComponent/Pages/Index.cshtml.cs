using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AjaxRazorComponent.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty] public string Greeting { get; set; }
        public string Message { get; set; }

        public void OnGet() { }

        public void OnPost()
        {
            Message = Greeting;
        }
    }
}

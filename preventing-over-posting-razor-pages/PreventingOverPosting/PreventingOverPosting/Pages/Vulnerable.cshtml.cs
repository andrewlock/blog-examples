using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PreventingOverPosting.Pages
{
    public class VulnerableModel : PageModel
    {
        private readonly AppUserService _users;

        public VulnerableModel(AppUserService users)
        {
            _users = users;
        }
        
        [BindProperty]
        public AppUser CurrentUser { get; set; }

        public IActionResult OnGet(int id)
        {
            CurrentUser = _users.Get(id);
            if(CurrentUser is null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _users.Upsert(id, CurrentUser);
            return Page();
        }
    }
}
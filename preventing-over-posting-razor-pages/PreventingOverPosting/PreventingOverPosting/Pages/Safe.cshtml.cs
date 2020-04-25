using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PreventingOverPosting.Pages
{
    public class SafeModel : PageModel
    {
        private readonly AppUserService _users;
        public SafeModel(AppUserService users)
        {
            _users = users;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public AppUser CurrentUser { get; set; }

        public IActionResult OnGet(int id)
        {
            CurrentUser = _users.Get(id);
            if (CurrentUser is null)
            {
                return NotFound();
            }
            Input = new InputModel { Name = CurrentUser.Name };
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                CurrentUser = _users.Get(id);
                return Page();
            }

            var user = _users.Get(id);
            user.Name = Input.Name;
            _users.Upsert(id, user);
            return RedirectToPage();
        }

        public class InputModel
        {
            public string Name { get; set; }

        }
    }
}
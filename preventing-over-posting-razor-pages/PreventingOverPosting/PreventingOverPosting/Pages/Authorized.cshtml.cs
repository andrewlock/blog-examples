using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PreventingOverPosting.Pages
{
    public class AuthorizedModel : PageModel
    {
        private readonly AppUserService _users;
        private readonly IAuthorizationService _auth;
        public AuthorizedModel(AppUserService users, IAuthorizationService auth)
        {
            _users = users;
            _auth = auth;
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var authResult = await _auth.AuthorizeAsync(User, id, "CanEditUser");
            if (!authResult.Succeeded)
            {
                // would normally send to access dendied page, but no auth registered in this simple example
                // return Forbid();
                return RedirectToPage("Error");
            }

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
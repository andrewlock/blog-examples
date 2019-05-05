using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomPasswordHasher.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        public IndexModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty, Required, MinLength(6), DisplayName("Enter a password. It will MD5 Hashed, rehashed, and stored")]
        public string Password { get; set; }

        [DisplayName("Previous hashed password value")]
        public string HashedPassword { get; set; }

        [TempData]
        public string Message { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            HashedPassword = user.PasswordHash;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                HashedPassword = user.PasswordHash;
                return Page();
            }

            var md5Password = Md5PasswordHasher<IdentityUser>.GetM5Hash(Password);

            await _userManager.SetMd5PasswordForUser(user, md5Password);

            Message = "Password Updated";

            return RedirectToPage();
        }
    }
}

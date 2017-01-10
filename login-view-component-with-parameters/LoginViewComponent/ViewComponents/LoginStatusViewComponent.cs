using System.Threading.Tasks;
using LoginViewComponent.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoginViewComponent
{
    public class LoginStatusViewComponent : ViewComponent
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginStatusViewComponent(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool shouldShowRegisterLink)
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                return View("LoggedIn", user);
            }
            else
            {
                var viewModel = new AnonymousViewModel
                {
                    IsRegisterLinkVisible = shouldShowRegisterLink
                };
                return View(viewModel);
            }
        }

        public class AnonymousViewModel
        {
            public bool IsRegisterLinkVisible { get; set; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.FeatureManagement;

namespace CustomFeatureFilter.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IFeatureManagerSnapshot _featureManager;

        public IndexModel(IFeatureManagerSnapshot featureManager)
        {
            _featureManager = featureManager;
        }

        public string WelcomeMessage { get; set; }

        public void OnGet()
        {
            WelcomeMessage = _featureManager.IsEnabled(FeatureFlags.NewWelcomeBanner)
                ? "Welcome to the Beta"
                : "Welcome";
        }
    }
}

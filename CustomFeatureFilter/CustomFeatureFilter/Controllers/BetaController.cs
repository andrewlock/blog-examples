using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.FeatureManagement.Mvc;

namespace CustomFeatureFilter
{
    [FeatureGate(FeatureFlags.Beta)]
    public class BetaController : Controller
    {
        [HttpGet("Beta")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

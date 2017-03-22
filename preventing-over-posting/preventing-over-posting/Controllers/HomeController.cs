using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace preventing_over_posting.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new UserModel());
        }

        [HttpPost]
        public IActionResult Vulnerable(UserModel model)
        {
            return View("Index", model);
        }

        public IActionResult Safe1([Bind(nameof(UserModel.Name))] UserModel model)
        {
            return View("Index", model);
        }

        public IActionResult Safe2(UserModelWithReadOnlyProperties model)
        {
            //note this will error as wrong model type
            return View("Index", model);
        }

        public IActionResult Safe3(BindingModel bindingModel)
        {
            var model = new UserModel();

            // can be simplified using AutoMapper
            model.Name = bindingModel.Name;
            model.IsAdmin = false; // no IsAdmin property on bindingModel

            return View("Index", model);
        }

        public IActionResult Safe4(BindingModel bindingModel)
        {
            var model = new DerivedUserModel();

            // can be simplified using AutoMapper
            model.Name = bindingModel.Name;
            model.IsAdmin = false; // no IsAdmin property on bindingModel

            //note this will error as wrong model type
            return View("Index", model);
        }

        public IActionResult Safe5(DistinctBindingModel bindingModel)
        {
            var model = new DerivedUserModel();

            // can be simplified using AutoMapper
            model.Name = bindingModel.Name;
            model.IsAdmin = false; // no IsAdmin property on bindingModel

            //note this will error as wrong model type
            return View("Index", model);
        }
    }
}

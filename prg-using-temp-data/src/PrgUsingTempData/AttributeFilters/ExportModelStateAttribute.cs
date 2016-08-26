using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace PrgUsingTempData.AttributeFilters
{
    public class ExportModelStateAttribute : ModelStateTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Only export when ModelState is not valid
            if (!filterContext.ModelState.IsValid)
            {
                //Export if we are redirecting
                if (filterContext.Result is RedirectResult 
                    || filterContext.Result is RedirectToRouteResult 
                    || filterContext.Result is RedirectToActionResult)
                {
                    var controller = filterContext.Controller as Controller;
                    if (controller != null && filterContext.ModelState != null)
                    {
                        controller.TempData[Key] = SerialiseModelState(filterContext.ModelState);
                    }
                }
            }

            base.OnActionExecuted(filterContext);
        }

        private static string SerialiseModelState(ModelStateDictionary modelState)
        {
            var errorList = modelState
                .Select(kvp => new ModelStateValue
                               {
                                   Key = kvp.Key,
                                   AttemptedValue = kvp.Value.AttemptedValue,
                                   RawValue = kvp.Value.RawValue,
                                   ErrorMessages = kvp.Value.Errors.Select(err => err.ErrorMessage).ToList(),
                               });

            return JsonConvert.SerializeObject(errorList);
        }
    }
}
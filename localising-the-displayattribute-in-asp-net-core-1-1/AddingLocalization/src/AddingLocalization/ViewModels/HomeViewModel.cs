using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddingLocalization.ViewModels
{
    public class HomeViewModel
    {
        [Required(ErrorMessage = ResourceKeys.Required)]
        [EmailAddress(ErrorMessage = ResourceKeys.NotAValidEmail)]
        [Display(Name = ResourceKeys.YourEmail)]
        public string Email { get; set; }
    }
}

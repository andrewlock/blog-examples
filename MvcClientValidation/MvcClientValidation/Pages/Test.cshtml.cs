using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MvcClientValidation.Pages;

public class TestModel : PageModel
{
    [BindProperty]
    public Person Input { get; set; }

    public ActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        TempData["Message"] = $"Hello {Input.Name} ({Input.Email})!";
        return RedirectToPage("Index");
    }

    public class Person
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [EndsWithValidation("@mycompany.com")]
        public string Email { get; set; }
    }
}

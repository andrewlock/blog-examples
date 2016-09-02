using System.ComponentModel.DataAnnotations;

namespace PrgUsingTempData.ViewModels
{
    public class EditModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress, MinLength(20)]
        public string Email { get; set; }
    }
}
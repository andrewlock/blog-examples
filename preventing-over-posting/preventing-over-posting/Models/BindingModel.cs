using System.ComponentModel.DataAnnotations;

namespace preventing_over_posting
{
    public class BindingModel
    {
        [MaxLength(200)]
        [Display(Name = "Full name")]
        [Required]
        public string Name { get; set; }
    }

    public class DerivedUserModel: BindingModel
    {
        public bool IsAdmin { get; set; }
    }
}

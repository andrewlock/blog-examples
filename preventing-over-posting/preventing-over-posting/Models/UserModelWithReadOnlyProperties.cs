using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace preventing_over_posting
{
    public class UserModelWithReadOnlyProperties
    {
        [MaxLength(200)]
        [Display(Name = "Full name")]
        [Required]
        public string Name { get; set; }

        [Editable(false)]
        public bool IsAdmin { get; set; }
    }

}

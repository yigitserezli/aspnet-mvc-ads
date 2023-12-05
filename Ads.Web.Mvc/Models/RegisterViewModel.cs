using System.ComponentModel.DataAnnotations;

namespace Ads.Web.Mvc.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name Range should be 3-30 Characters")]
        public string Name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Surname Range should be 3-30 Characters")]
        public string Surname { get; set; }

        [Required]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter email adress")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid email format.")]
        public string Username { get; set; } = string.Empty;


        [Required, MinLength(4)]
        public string Password { get; set; } = string.Empty;

        [Required, Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
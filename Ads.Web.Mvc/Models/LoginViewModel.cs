using System.ComponentModel.DataAnnotations;

namespace Ads.Web.Mvc.Models
{
    public class LoginViewModel
    {
        [Required, MinLength(1)]
        public string Username { get; set; } = string.Empty;

        [Required, MinLength(1)]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
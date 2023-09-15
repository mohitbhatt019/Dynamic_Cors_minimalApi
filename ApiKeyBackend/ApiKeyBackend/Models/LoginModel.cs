using System.ComponentModel.DataAnnotations;

namespace ApiKeyBackend.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter Username")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wrong password")]
        public string Password { get; set; } = string.Empty;
    }
}

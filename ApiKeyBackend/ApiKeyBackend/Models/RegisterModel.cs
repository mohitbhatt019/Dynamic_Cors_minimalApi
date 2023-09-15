using System.ComponentModel.DataAnnotations;

namespace ApiKeyBackend.Models
{
    public class RegisterModel
    {
        [Required (ErrorMessage ="Please enter Username")]
        public string UserName { get; set; }=string.Empty;

        [Required (ErrorMessage ="Wrong password")]
        public string Password { get; set; }=string.Empty;

        [EmailAddress(ErrorMessage ="Enter email address in correct format")]
        public string Email { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace src.Models.Authentication
{
    public class RegisterViewModel
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
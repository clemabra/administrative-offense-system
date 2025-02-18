namespace src.Models.Authentication
{
    public class RegisterModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class LoginModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class ValidateSSOTokenRequest
    {
        public required string SSOToken { get; set; }
    }
}
namespace src.Models.Authentication
{
    public class SSOToken
    {
        //TODO maybe int and not Guid
        public required Guid Id { get; set; }
        public required string UserId { get; set; }
        public required string Token { get; set; }
        public required DateTime ExpiryDate { get; set; }
        public required bool IsUsed { get; set; }
        public bool IsExpired => DateTime.UtcNow > ExpiryDate;
    }
}
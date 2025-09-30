namespace PortfolioATS.Core.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public bool IsActive { get; set; } = true;
        public DateTime? LastLogin { get; set; }
        public string ProfileId { get; set; } = string.Empty; // Referência ao Profile
    }
}
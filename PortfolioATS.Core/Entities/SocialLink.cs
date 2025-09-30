namespace PortfolioATS.Core.Entities
{
    public class SocialLink
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Platform { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Username { get; set; }
    }
}
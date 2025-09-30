namespace PortfolioATS.Core.Entities
{
    public class Language
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Proficiency { get; set; } = string.Empty;
    }
}
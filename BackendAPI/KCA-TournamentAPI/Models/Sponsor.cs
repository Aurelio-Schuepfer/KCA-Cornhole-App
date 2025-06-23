namespace KCA_TournamentAPI.Models
{
    public class Sponsor
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? LogoUrl { get; set; }
        public int TournamentId { get; set; }
        public Tournament? Tournament { get; set; }
    }
}

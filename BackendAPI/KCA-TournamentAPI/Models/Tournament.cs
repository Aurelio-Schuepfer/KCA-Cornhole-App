namespace KCA_TournamentAPI.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Organizer { get; set; }
        public string? Location { get; set; }
        public string? MeetingPoint { get; set; }
        public DateTime? Date { get; set; }
        public int? MaxTeams { get; set; }
        public decimal? EntryFee { get; set; }
        public bool Private { get; set; }
        public string? Format { get; set; }
        public string? Rules { get; set; }
        public string? Notes { get; set; }
        public string? AgeGroup { get; set; }
        public string? SkillLevel { get; set; }
        public string? Prize { get; set; }
        public string? League { get; set; }
        public List<Sponsor> Sponsors { get; set; } = new();
        public List<Partner> Partners { get; set; } = new();
        public List<Team> Teams { get; set; } = new();
        public string? Administrator { get; set; }
        public List<Match> Matches { get; set; } = new();
        public TournamentStats? Stats { get; set; }
        public List<Participant> Participants { get; set; } = new();
    }
}
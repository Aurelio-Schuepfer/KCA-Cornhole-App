namespace KCA_TournamentAPI.Models;

public class Match
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public Tournament Tournament { get; set; } = null!;

    public int TeamAId { get; set; }
    public Team TeamA { get; set; } = null!;

    public int TeamBId { get; set; }
    public Team TeamB { get; set; } = null!;

    public DateTime StartTime { get; set; }
    public string Location { get; set; } = null!;
    public string Round { get; set; } = null!;

    public int? ScoreTeamA { get; set; }
    public int? ScoreTeamB { get; set; }

    public string Status { get; set; } = "Scheduled";
}

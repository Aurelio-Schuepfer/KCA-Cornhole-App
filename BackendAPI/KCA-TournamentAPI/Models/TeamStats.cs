namespace KCA_TournamentAPI.Models;

public class TeamStats
{
    public int Id { get; set; }
    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;

    public int MatchesPlayed { get; set; }
    public int MatchesWon { get; set; }
    public int MatchesLost { get; set; }

    public int TournamentsPlayed { get; set; }
    public int TournamentsWon { get; set; }

    public int TotalPointsScored { get; set; }
    public int TotalPointsReceived { get; set; }
}

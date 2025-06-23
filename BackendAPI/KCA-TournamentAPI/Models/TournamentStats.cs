namespace KCA_TournamentAPI.Models;

public class TournamentStats
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public Tournament Tournament { get; set; } = null!;

    public int TotalTeams { get; set; }
    public int TotalMatches { get; set; }

    public int MatchesPlayed { get; set; }
    public int MatchesFinished { get; set; }

    public int TotalPointsScored { get; set; }

    public int? WinningTeamId { get; set; }
    public Team? WinningTeam { get; set; }
}

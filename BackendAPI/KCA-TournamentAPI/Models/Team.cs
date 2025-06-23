namespace KCA_TournamentAPI.Models;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string LogoUrl { get; set; } = null!;

    public List<Participant> Players { get; set; } = new();
    public List<Tournament> Tournaments { get; set; } = new();
    public TeamStats Stats { get; set; } = new();

}

using KCA_TournamentAPI.Models;

public class Participant
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Team { get; set; } = "";
    public string Avatar { get; set; } = "";
    public int TournamentId { get; set; }
    public Tournament? Tournament { get; set; }
}
namespace KCA_TournamentAPI.Models
{
    public class Bracket
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; } = null!;
        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;

        public int? NextMatchId { get; set; }
        public Match? NextMatch { get; set; }

        public string BracketType { get; set; } = "SingleElimination";
        public int RoundNumber { get; set; }
    }
}

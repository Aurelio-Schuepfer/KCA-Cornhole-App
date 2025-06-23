using KCA_TournamentAPI.Data;
using KCA_TournamentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KCA_TournamentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TournamentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TournamentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Tournament>>> GetTournaments()
        {
            return await _context.Tournaments
                .Include(t => t.Sponsors)
                .Include(t => t.Partners)
                .Include(t => t.Teams)
                .Include(t => t.Matches)
                .Include(t => t.Stats)
                .Include(t => t.Participants)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tournament>> GetTournament(int id)
        {
            var tournament = await _context.Tournaments
                .Include(t => t.Sponsors)
                .Include(t => t.Partners)
                .Include(t => t.Teams)
                .Include(t => t.Matches)
                .Include(t => t.Stats)
                .Include(t => t.Participants)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tournament == null)
                return NotFound();

            return tournament;
        }

        [HttpPost]
        public async Task<ActionResult<Tournament>> CreateTournament(Tournament tournament)
        {
            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTournament), new { id = tournament.Id }, tournament);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTournament(int id, Tournament tournament)
        {
            if (id != tournament.Id)
                return BadRequest();

            var existingTournament = await _context.Tournaments
                .Include(t => t.Participants)
                .Include(t => t.Sponsors)
                .Include(t => t.Partners)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingTournament == null)
                return NotFound();

            // Update einfache Felder
            existingTournament.Name = tournament.Name;
            existingTournament.Organizer = tournament.Organizer;
            existingTournament.Location = tournament.Location;
            existingTournament.MeetingPoint = tournament.MeetingPoint;
            existingTournament.Date = tournament.Date;
            existingTournament.MaxTeams = tournament.MaxTeams;
            existingTournament.EntryFee = tournament.EntryFee;
            existingTournament.Private = tournament.Private;
            existingTournament.Format = tournament.Format;
            existingTournament.Rules = tournament.Rules;
            existingTournament.Notes = tournament.Notes;
            existingTournament.AgeGroup = tournament.AgeGroup;
            existingTournament.SkillLevel = tournament.SkillLevel;
            existingTournament.Prize = tournament.Prize;
            existingTournament.League = tournament.League;
            existingTournament.Administrator = tournament.Administrator;

            // Participants ersetzen
            existingTournament.Participants.Clear();
            if (tournament.Participants != null)
            {
                foreach (var p in tournament.Participants)
                {
                    if (!string.IsNullOrWhiteSpace(p.Name))
                    {
                        existingTournament.Participants.Add(new Participant
                        {
                            Name = p.Name,
                            Team = p.Team ?? "",
                            Avatar = p.Avatar ?? "",
                            TournamentId = id
                        });
                    }
                }
            }

            // Sponsors ersetzen
            existingTournament.Sponsors.Clear();
            if (tournament.Sponsors != null)
            {
                foreach (var s in tournament.Sponsors)
                {
                    if (!string.IsNullOrWhiteSpace(s.Name))
                    {
                        existingTournament.Sponsors.Add(new Sponsor
                        {
                            Name = s.Name,
                            LogoUrl = s.LogoUrl,
                            TournamentId = id
                        });
                    }
                }
            }

            // Partners ersetzen
            existingTournament.Partners.Clear();
            if (tournament.Partners != null)
            {
                foreach (var p in tournament.Partners)
                {
                    if (!string.IsNullOrWhiteSpace(p.Name))
                    {
                        existingTournament.Partners.Add(new Partner
                        {
                            Name = p.Name,
                            LogoUrl = p.LogoUrl,
                            TournamentId = id
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null)
                return NotFound();

            _context.Tournaments.Remove(tournament);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

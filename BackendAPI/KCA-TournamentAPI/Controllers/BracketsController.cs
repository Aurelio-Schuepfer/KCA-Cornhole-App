using KCA_TournamentAPI.Data;
using KCA_TournamentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KCA_TournamentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BracketsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BracketsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Bracket>>> GetBrackets()
        {
            return await _context.Brackets.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bracket>> GetBracket(int id)
        {
            var bracket = await _context.Brackets.FindAsync(id);
            if (bracket == null) return NotFound();
            return bracket;
        }

        [HttpPost]
        public async Task<ActionResult<Bracket>> CreateBracket(Bracket bracket)
        {
            _context.Brackets.Add(bracket);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBracket), new { id = bracket.Id }, bracket);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBracket(int id, Bracket bracket)
        {
            if (id != bracket.Id) return BadRequest();
            _context.Entry(bracket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Brackets.Any(b => b.Id == id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBracket(int id)
        {
            var bracket = await _context.Brackets.FindAsync(id);
            if (bracket == null) return NotFound();

            _context.Brackets.Remove(bracket);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

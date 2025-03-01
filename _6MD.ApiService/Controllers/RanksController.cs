using DataStructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _6MD.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RanksController : ControllerBase
    {
        private readonly DB _context;

        public RanksController(DB context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rank>>> GetRanks()
        {
            return await _context.Ranks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rank>> GetRank(int id)
        {
            var rank = await _context.Ranks
                .FirstOrDefaultAsync(r => r.ID == id);
            if (rank == null)
            {
                return NotFound(new { message = "Rank not found" });
            }
            return rank;
        }

        [HttpPost]
        public async Task<ActionResult<Rank>> CreateRank(Rank rank)
        {
            _context.Ranks.Add(rank);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRanks), new { id = rank.ID }, rank);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRank(int id, Rank rank)
        {
            if (id != rank.ID)
                return BadRequest(new { message = "Invalid rank data" });
            if (!await _context.Ranks.AnyAsync(r => r.ID == id))
                return NotFound(new { message = "Rank not found" });
            _context.Ranks.Update(rank);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Rank>> DeleteRank(int id)
        {
            if (!await _context.Ranks.AnyAsync(r => r.ID == id))
                return NotFound(new { message = "Rank not found" });
            await _context.Ranks.Where(r => r.ID == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

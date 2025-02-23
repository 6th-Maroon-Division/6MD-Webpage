using DataStructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _6MD.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LOAController : ControllerBase
    {
        private readonly DB _context;

        public LOAController(DB context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LOA>>> GetLOAs()
        {
            return await _context.LOAs.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LOA>> GetLOA(int id)
        {
            var loa = await _context.LOAs
                .FirstOrDefaultAsync(l => l.ID == id);
            if (loa == null)
            {
                return NotFound(new { message = "LOA not found" });
            }
            return loa;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLOA(int id, LOA loa)
        {
            if (id != loa.ID)
                return BadRequest(new { message = "Invalid LOA data" });
            if (!await _context.LOAs.AnyAsync(l => l.ID == id))
                return NotFound(new { message = "LOA not found" });
            _context.LOAs.Update(loa);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<LOA>> CreateLOA(LOA loa)
        {
            _context.LOAs.Add(loa);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLOAs), new { id = loa.ID }, loa);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<LOA>> DeleteLOA(int id)
        {
            if (!await _context.LOAs.AnyAsync(l => l.ID == id))
                return NotFound(new { message = "LOA not found" });
            await _context.LOAs.Where(l => l.ID == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

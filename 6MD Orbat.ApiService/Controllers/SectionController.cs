using DataStructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _6MD.ApiService.Controllers
{
    [Route("api/[controller]")]
    public class SectionController: ControllerBase
    {
        private readonly DB _context;

        public SectionController(DB context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Section>>> GetSections()
        {
            return await _context.Sections.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Section>> GetSection(int id)
        {
            var section = await _context.Sections
                .Include(s => s.Slots)
                .Include(s => s.Operation)
                .FirstOrDefaultAsync(s => s.ID == id);
            if (section == null)
            {
                return NotFound(new { message = "Section not found" });
            }
            return section;
        }

        [HttpGet("Opperation/{id}")]
        public async Task<ActionResult<IEnumerable<Section>>> GetSectionsByOpperation(int id)
        {
            var sections = await _context.Sections
                .Include(s => s.Slots)
                .Include(s => s.Operation)
                .Where(s => s.OperationID == id)
                .ToListAsync();
            return sections;
        }

        [HttpPost]
        public async Task<ActionResult<Section>> CreateSection(Section section)
        {
            _context.Sections.Add(section);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSections), new { id = section.ID }, section);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSection(int id, Section section)
        {
            if (id != section.ID)
                return BadRequest(new { message = "Invalid section data" });
            if (!await _context.Sections.AnyAsync(s => s.ID == id))
                return NotFound(new { message = "Section not found" });
            _context.Sections.Update(section);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Section>> DeleteSection(int id)
        {
            if (!await _context.Sections.AnyAsync(s => s.ID == id))
                return NotFound(new { message = "Section not found" });
            await _context.Sections.Where(s => s.ID == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

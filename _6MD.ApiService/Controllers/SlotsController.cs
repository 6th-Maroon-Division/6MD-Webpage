using DataStructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _6MD.ApiService.Controllers
{
    [Route("api/[controller]")]
    public class SlotsController : ControllerBase
    {
        private readonly DB _context;

        public SlotsController(DB context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Slot>>> GetSlots()
        {
            return await _context.Slots.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Slot>> GetSlot(int id)
        {
            var slot = await _context.Slots
                .Include(s => s.Section)
                .FirstOrDefaultAsync(s => s.ID == id);
            if (slot == null)
            {
                return NotFound(new { message = "Slot not found" });
            }
            return slot;
        }
        [HttpGet("section/{id}")]
        public async Task<ActionResult<IEnumerable<Slot>>> GetSlotsBySection(int id)
        {
            var slots = await _context.Slots
                .Include(s => s.Section)
                .Where(s => s.SectionID == id)
                .ToListAsync();
            return slots;
        }
        [HttpGet("Opperation/{id}")]
        public async Task<ActionResult<IEnumerable<Slot>>> GetSlotsByOpperation(int id)
        {
            var slots = await _context.Slots
                .Include(s => s.Section)
                .Where(s => s.Section.OperationID == id)
                .ToListAsync();
            return slots;
        }
        [HttpPost]
        public async Task<ActionResult<Slot>> CreateSlot(Slot slot)
        {
            _context.Slots.Add(slot);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSlots), new { id = slot.ID }, slot);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSlot(int id, Slot slot)
        {
            if (id != slot.ID)
                return BadRequest(new { message = "Invalid slot data" });
            if (!await _context.Slots.AnyAsync(s => s.ID == id))
                return NotFound(new { message = "Slot not found" });
            _context.Slots.Update(slot);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Slot>> DeleteSlot(int id)
        {
            if (!await _context.Slots.AnyAsync(s => s.ID == id))
                return NotFound(new { message = "Slot not found" });
            await _context.Slots.Where(s => s.ID == id).ExecuteDeleteAsync();
            return NoContent();
        }
    }
}

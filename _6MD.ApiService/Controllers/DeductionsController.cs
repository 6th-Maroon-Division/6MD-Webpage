using DataStructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _6MD.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeductionsController : ControllerBase
    {
        private readonly DB _context;

        public DeductionsController(DB context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deduction>>> GetDeductions()
        {
            return await _context.Deductions.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Deduction>> GetDeduction(int id)
        {
            var deduction = await _context.Deductions
                .Include(d => d.User)
                .Include(d => d.DeductedBy)
                .FirstOrDefaultAsync(d => d.ID == id);
            if (deduction == null)
            {
                return NotFound(new { message = "Deduction not found" });
            }
            return deduction;
        }


        [HttpPost]
        public async Task<ActionResult<Deduction>> CreateDeduction(Deduction deduction)
        {
            _context.Deductions.Add(deduction);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDeductions), new { id = deduction.ID }, deduction);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Deduction>> DeleteDeduction(int id)
        {
            if (!await _context.Deductions.AnyAsync(d => d.ID == id))
                return NotFound(new { message = "Deduction not found" });
            await _context.Deductions.Where(d => d.ID == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeduction(int id, Deduction deduction)
        {
            if (id != deduction.ID)
            {
                return BadRequest(new { message = "Invalid ID" });
            }
            if (!await _context.Deductions.AnyAsync(d => d.ID == id))
            {
                return NotFound(new { message = "Deduction not found" });
            }
            _context.Entry(deduction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

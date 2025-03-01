using DataStructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _6MD.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly DB _context;

        public OperationsController(DB context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operation>>> GetOperations()
        {
            return await _context.Operations.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Operation>> GetOperation(int id)
        {
            var operation = await _context.Operations
                .Include(o => o.Sections)
                .Include(o => o.OPAttendances)
                .Include(o => o.Users)
                .FirstOrDefaultAsync(o => o.ID == id);
            if (operation == null)
            {
                return NotFound(new { message = "Operation not found" });
            }
            return operation;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOperation(int id, Operation operation)
        {
            if (id != operation.ID)
                return BadRequest(new { message = "Invalid operation data" });
            if (!await _context.Operations.AnyAsync(o => o.ID == id))
                return NotFound(new { message = "Operation not found" });
            _context.Operations.Update(operation);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Operation>> CreateOperation(Operation operation)
        {
            _context.Operations.Add(operation);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOperations), new { id = operation.ID }, operation);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Operation>> DeleteOperation(int id)
        {
            if (!await _context.Operations.AnyAsync(o => o.ID == id))
                return NotFound(new { message = "Operation not found" });
            await _context.Operations.Where(o => o.ID == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

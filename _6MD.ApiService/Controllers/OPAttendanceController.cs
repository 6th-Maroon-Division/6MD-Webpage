using DataStructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _6MD.ApiService.Controllers
{
    public class OPAttendanceController : ControllerBase
    {
        private readonly DB _context;
        public OPAttendanceController(DB context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OPAttendance>>> GetOPAttendance()
        {
            return await _context.OPAttendances.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OPAttendance>> GetOPAttendance(int id)
        {
            var attendance = await _context.OPAttendances
                .Include(a => a.User1)
                .Include(a => a.Operation)
                .FirstOrDefaultAsync(a => a.ID == id);
            if (attendance == null)
            {
                return NotFound(new { message = "Attendance not found" });
            }
            return attendance;
        }
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<OPAttendance>>> GetOPAttendanceByUser(int id)
        {
            var attendances = await _context.OPAttendances
                .Include(a => a.User1)
                .Include(a => a.Operation)
                .Where(a => a.UserID == id)
                .ToListAsync();
            return attendances;
        }
        [HttpGet("operation/{id}")]
        public async Task<ActionResult<IEnumerable<OPAttendance>>> GetOPAttendanceByOperation(int id)
        {
            var attendances = await _context.OPAttendances
                .Include(a => a.User1)
                .Include(a => a.Operation)
                .Include(a => a.Slot)
                .ThenInclude(s => s.Section)
                .Where(a => a.OperationID == id)
                .ToListAsync();
            return attendances;
        }
        [HttpPost]
        public async Task<ActionResult<OPAttendance>> CreateOPAttendance(OPAttendance attendance)
        {
            _context.OPAttendances.Add(attendance);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOPAttendance), new { id = attendance.ID }, attendance);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOPAttendance(int id, OPAttendance attendance)
        {
            if (id != attendance.ID)
                return BadRequest(new { message = "Invalid attendance data" });
            if (!await _context.OPAttendances.AnyAsync(a => a.ID == id))
                return NotFound(new { message = "OPAttendance not found" });
            _context.OPAttendances.Update(attendance);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<OPAttendance>> DeleteOPAttendance(int id)
        {
            if (!await _context.OPAttendances.AnyAsync(a => a.ID == id))
                return NotFound(new { message = "OPAttendance not found" });
            await _context.OPAttendances.Where(a => a.ID == id).ExecuteDeleteAsync();
            return NoContent();
        }
    }
}

using DataStructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _6MD.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly DB _context;

        public AttendanceController(DB context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendance()
        {
            return await _context.Attendances.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Attendance>> GetAttendance(int id)
        {
            var attendance = await _context.Attendances
                .Include(a => a.User)
                .Include(a => a.AttendanceTakenBy)
                .FirstOrDefaultAsync(a => a.ID == id);
            if (attendance == null)
            {
                return NotFound(new { message = "Attendance not found" });
            }
            return attendance;
        }
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendanceByUser(int id)
        {
            var attendances = await _context.Attendances
                .Include(a => a.User)
                .Include(a => a.AttendanceTakenBy)
                .Where(a => a.UserID == id)
                .ToListAsync();
            return attendances;
        }
        [HttpGet("takenby/{id}")]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendanceByTakenBy(int id)
        {
            var attendances = await _context.Attendances
                .Include(a => a.User)
                .Include(a => a.AttendanceTakenBy)
                .Where(a => a.AttendanceTakenByID == id)
                .ToListAsync();
            return attendances;
        }

        [HttpPost]
        public async Task<ActionResult<Attendance>> CreateAttendance(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAttendance), new { id = attendance.ID }, attendance);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttendance(int id, Attendance attendance)
        {
            if (id != attendance.ID)
                return BadRequest(new { message = "Invalid attendance data" });
            if (!await _context.Attendances.AnyAsync(a => a.ID == id))
                return NotFound(new { message = "Attendance not found" });
            _context.Attendances.Update(attendance);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Attendance>> DeleteAttendance(int id)
        {
            if (!await _context.Attendances.AnyAsync(a => a.ID == id))
                return NotFound(new { message = "Attendance not found" });
            await _context.Attendances.Where(a => a.ID == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

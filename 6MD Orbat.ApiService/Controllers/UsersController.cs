using DataStructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _6MD_Orbat.ApiService.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly DataStructure.DB _context;

        public UsersController(DataStructure.DB context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Rank) // Include Rank data
                .Include(u => u.Groups) // Include Groups data
                .Include(u => u.Trainers) // Include Trainers data
                .ToListAsync();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users
                .Include(u => u.Rank)
                .Include(u => u.Groups)
                .Include(u => u.Trainers)
                .Include(u => u.Trainings)
                .Include(u => u.OPAttendances)
                .Include(u => u.Attendances)
                .Include(u => u.Deductions)
                .Include(u => u.DeductionsGiven)
                .Include(u => u.AttendanceTaken)
                .FirstOrDefaultAsync(u => u.ID == id);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest(new { message = "Invalid user data" });

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.ID }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.ID == id);
            if (existingUser == null)
                return NotFound(new { message = "User not found" });

            existingUser.Name = updatedUser.Name;
            existingUser.DateJoined = updatedUser.DateJoined;
            existingUser.Retired = updatedUser.Retired;
            existingUser.DiscordID = updatedUser.DiscordID;
            existingUser.UserPremissions = updatedUser.UserPremissions;
            existingUser.RankID = updatedUser.RankID;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.ID == id);
            if (user == null)
                return NotFound(new { message = "User not found" });
            await _context.Users.Where(u => u.ID == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            user = await _context.Users
                .FirstOrDefaultAsync(u => u.ID == id);
            if (!(user == null))
                return Problem ("User Still existing");

            return NoContent();
        }
    }
}

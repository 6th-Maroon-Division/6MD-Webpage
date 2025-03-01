using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataStructure;

namespace _6MD.ApiService.Controllers
{
    [ApiController]
    [Route("api/group")]
    public class GroupController : ControllerBase
    {
        private readonly DataStructure.DB _context;
        public GroupController(DataStructure.DB context)
        {
            _context = context;
        }
        // GET: api/group
        [HttpGet]
        public async Task<IActionResult> GetGroups()
        {
            var groups = await _context.Groups
                .Include(g => g.Users) // Include GroupMembers data
                .ToListAsync();
            return Ok(groups);
        }
        // GET: api/group/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(int id)
        {
            var group = await _context.Groups
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.ID == id);
            if (group == null)
                return NotFound(new { message = "Group not found" });
            return Ok(group);
        }
        // POST: api/group
        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] Groups group)
        {
            if (group == null)
                return BadRequest(new { message = "Invalid group data" });
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGroupById), new { id = group.ID }, group);
        }
        // PUT: api/group/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(int id, [FromBody] Groups group)
        {
            if (group == null)
                return BadRequest(new { message = "Invalid group data" });
            var existingGroup = await _context.Groups.FirstOrDefaultAsync(g => g.ID == id );
            if (existingGroup == null)
                return NotFound(new { message = "Group not found" });
            existingGroup.Name = group.Name;
            existingGroup.Premissions = group.Premissions;
            existingGroup.DiscordRoleID = group.DiscordRoleID;
            existingGroup.RoleColor = group.RoleColor;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/group/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            if (!await _context.Groups.AnyAsync(g => g.ID == id))
                return NotFound(new { message = "Group not found" });
            await _context.Groups.Where(g => g.ID == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/group/{groupId}/addUser/{userId}
        [HttpPost("{groupId}/addUser/{userId}")]
        public async Task<IActionResult> AddUserToGroup(int groupId, int userId)
        {
            var group = await _context.Groups.Include(g => g.Users).FirstOrDefaultAsync(g => g.ID == groupId);
            if (group == null)
                return NotFound(new { message = "Group not found" });
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ID == userId);
            if (user == null)
                return NotFound(new { message = "User not found" });
            group.Users.Add(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/group/{groupId}/removeUser/{userId}
        [HttpPost("{groupId}/removeUser/{userId}")]
        public async Task<IActionResult> RemoveUserFromGroup(int groupId, int userId)
        {
            var group = await _context.Groups.Include(g => g.Users).FirstOrDefaultAsync(g => g.ID == groupId);
            if (group == null)
                return NotFound(new { message = "Group not found" });
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ID == userId);
            if (user == null)
                return NotFound(new { message = "User not found" });
            group.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

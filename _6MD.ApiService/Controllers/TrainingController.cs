using DataStructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _6MD.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingController : ControllerBase
    {
        private readonly DB _context;

        public TrainingController(DB context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Training>>> GetTraining()
        {
            return await _context.Trainings.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Training>> GetTraining(int id)
        {
            var training = await _context.Trainings
                .FirstOrDefaultAsync(t => t.ID == id);
            if (training == null)
            {
                return NotFound(new { message = "Training not found" });
            }
            return training;
        }

        [HttpPost]
        public async Task<ActionResult<Training>> CreateTraining(Training training)
        {
            _context.Trainings.Add(training);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTraining), new { id = training.ID }, training);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTraining(int id, Training training)
        {
            if (id != training.ID)
                return BadRequest(new { message = "Invalid training data" });
            if (!await _context.Trainings.AnyAsync(t => t.ID == id))
                return NotFound(new { message = "Training not found" });
            _context.Trainings.Update(training);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Training>> DeleteTraining(int id)
        {
            if (!await _context.Trainings.AnyAsync(t => t.ID == id))
                return NotFound(new { message = "Training not found" });
            await _context.Trainings.Where(t => t.ID == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

using DataStructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _6MD.ApiService.Controllers
{
    [Route("api/[controller]")]
    public class TrainersController: ControllerBase
    {
        private readonly DB _context;
        public TrainersController(DB context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trainers>>> GetTrainers()
        {
            return await _context.Trainers.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Trainers>> GetTrainer(int id)
        {
            var trainer = await _context.Trainers
                .FirstOrDefaultAsync(t => t.ID == id);
            if (trainer == null)
            {
                return NotFound(new { message = "Trainer not found" });
            }
            return trainer;
        }
        [HttpGet("Trainings/{id}")]
        public async Task<ActionResult<IEnumerable<Trainers>>> GetTrainings(int id)
        {
            var trainers = await _context.Trainers
                .Where(t => t.TrainingID == id)
                .ToListAsync();
            if (trainers == null)
            {
                return NotFound(new { message = "Trainers not found" });
            }
            return trainers;
        }
        [HttpPost]
        public async Task<ActionResult<Trainers>> CreateTrainer(Trainers trainer)
        {
            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTrainers), new { id = trainer.ID }, trainer);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainer(int id, Trainers trainer)
        {
            if (id != trainer.ID)
                return BadRequest(new { message = "Invalid Trainer data" });
            if (!await _context.Trainers.AnyAsync(t => t.ID == id))
                return NotFound(new { message = "Trainer not found" });
            _context.Trainers.Update(trainer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Trainers>> DeleteTrainer(int id)
        {
            if (!await _context.Trainers.AnyAsync(t => t.ID == id))
                return NotFound(new { message = "Trainer not found" });
            await _context.Trainers.Where(t => t.ID == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

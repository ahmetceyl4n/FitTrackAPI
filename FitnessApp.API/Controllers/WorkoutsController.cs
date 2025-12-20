using FitnessApp.Application.DTOs.WorkoutDTOs;
using FitnessApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutsController : ControllerBase
    {
        private readonly IWorkoutService _service;

        public WorkoutsController(IWorkoutService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWorkoutDto createDto)
        {
            var id = await _service.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = id }, new { id = id });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateWorkoutDto updateDto)
        {
            await _service.UpdateAsync(updateDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("add-exercise")] // Adres: api/Workouts/add-exercise
        public async Task<IActionResult> AddExercise(AddExerciseToWorkoutDto dto)
        {
            await _service.AddExerciseAsync(dto);
            return Ok(new { message = "Egzersiz ve setler antrenmana başarıyla eklendi!" });
        }

        [HttpDelete("{workoutId}/exercises/{exerciseId}")]
        public async Task<IActionResult> RemoveExercise(Guid workoutId, Guid exerciseId)
        {
            await _service.RemoveExerciseFromWorkoutAsync(workoutId, exerciseId);
            return NoContent(); // 204 No Content (Başarılı ama geriye veri dönmüyorum demek)
        }

        [HttpPut("sets")]
        public async Task<IActionResult> UpdateSet(UpdateSetDto dto)
        {
            await _service.UpdateSetAsync(dto);
            return NoContent();
        }

        [HttpDelete("sets/{id}")]
        public async Task<IActionResult> DeleteSet(Guid id)
        {
            await _service.DeleteSetAsync(id);
            return NoContent();
        }

    }
}
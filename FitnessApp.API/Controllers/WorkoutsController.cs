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
            if (result.Succeeded)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result.Succeeded)
            {
                return Ok(result.Data);
            }
            return NotFound(new { Message = result.ErrorMessage });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWorkoutDto createDto)
        {
            var result = await _service.CreateAsync(createDto);
            
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data }, result.Data);
            }
            // Validation hataları liste halinde olabilir
            if (result.Errors != null && result.Errors.Any())
            {
                return BadRequest(new { Messages = result.Errors });
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateWorkoutDto updateDto)
        {
            var result = await _service.UpdateAsync(updateDto);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }

        [HttpPost("add-exercise")] // Adres: api/Workouts/add-exercise
        public async Task<IActionResult> AddExercise(AddExerciseToWorkoutDto dto)
        {
            var result = await _service.AddExerciseAsync(dto);
            if (result.Succeeded)
            {
                return Ok(new { message = "Egzersiz ve setler antrenmana başarıyla eklendi!" });
            }
            // Validation hatası olabilir
             if (result.Errors != null && result.Errors.Any())
            {
                return BadRequest(new { Messages = result.Errors });
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }

        [HttpDelete("{workoutId}/exercises/{exerciseId}")]
        public async Task<IActionResult> RemoveExercise(Guid workoutId, Guid exerciseId)
        {
            var result = await _service.RemoveExerciseFromWorkoutAsync(workoutId, exerciseId);
            if (result.Succeeded)
            {
                return NoContent(); // 204 No Content (Başarılı ama geriye veri dönmüyorum demek)
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }

        [HttpPut("sets")]
        public async Task<IActionResult> UpdateSet(UpdateSetDto dto)
        {
            var result = await _service.UpdateSetAsync(dto);
             if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }

        [HttpDelete("sets/{id}")]
        public async Task<IActionResult> DeleteSet(Guid id)
        {
            var result = await _service.DeleteSetAsync(id);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(new { Message = result.ErrorMessage });
        }

    }
}
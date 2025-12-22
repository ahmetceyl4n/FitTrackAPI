using FitnessApp.Application.DTOs.ExerciseDTOs;
using FitnessApp.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FitnessApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExercisesController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExercisesController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _exerciseService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _exerciseService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound(); 
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateExerciseDto createExerciseDto)
        {
            var id = await _exerciseService.CreateAsync(createExerciseDto);

            return CreatedAtAction(nameof(GetById), new { id = id }, new { id = id });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateExerciseDto updateDto)
        {
            await _exerciseService.UpdateAsync(updateDto);
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _exerciseService.DeleteAsync(id);
            return NoContent();
        }





    }
}

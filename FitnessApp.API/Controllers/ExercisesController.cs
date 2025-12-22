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
            if (result.Succeeded)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _exerciseService.GetByIdAsync(id);
            if (result.Succeeded)
            {
                return Ok(result.Data);
            }
            return NotFound(new { Message = result.ErrorMessage });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateExerciseDto createExerciseDto)
        {
            var result = await _exerciseService.CreateAsync(createExerciseDto);

            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data }, result.Data);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateExerciseDto updateDto)
        {
            var result = await _exerciseService.UpdateAsync(updateDto);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _exerciseService.DeleteAsync(id);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.ErrorMessage);
        }





    }
}

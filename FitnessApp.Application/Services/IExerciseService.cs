using FitnessApp.Application.DTOs.ExerciseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Application.Services
{
    public interface IExerciseService
    {
        Task<List<ExerciseDto>> GetAllAsync();
        Task<ExerciseDto> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(CreateExerciseDto createDto);
        Task UpdateAsync(UpdateExerciseDto updateDto);
        Task DeleteAsync(Guid id);
    }
}

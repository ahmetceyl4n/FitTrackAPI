using FitnessApp.Application.DTOs.ExerciseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FitnessApp.Application.Common.Models; // Added

namespace FitnessApp.Application.Services
{
    public interface IExerciseService
    {
        Task<ServiceResult<List<ExerciseDto>>> GetAllAsync();
        Task<ServiceResult<ExerciseDto>> GetByIdAsync(Guid id);
        Task<ServiceResult<Guid>> CreateAsync(CreateExerciseDto createDto);
        Task<ServiceResult<bool>> UpdateAsync(UpdateExerciseDto updateDto); // Changed from Task to Task<ServiceResult<bool>>
        Task<ServiceResult<bool>> DeleteAsync(Guid id); // Changed from Task to Task<ServiceResult<bool>>
    }
}

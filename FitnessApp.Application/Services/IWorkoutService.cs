using FitnessApp.Application.Common.Models; // Added
using FitnessApp.Application.DTOs.WorkoutDTOs;

namespace FitnessApp.Application.Services
{
    public interface IWorkoutService
    {
        Task<ServiceResult<List<WorkoutDto>>> GetAllAsync();
        Task<ServiceResult<WorkoutDto>> GetByIdAsync(Guid id);
        Task<ServiceResult<Guid>> CreateAsync(CreateWorkoutDto createDto);
        Task<ServiceResult<bool>> UpdateAsync(UpdateWorkoutDto updateDto); // Changed to ServiceResult<bool>
        Task<ServiceResult<bool>> DeleteAsync(Guid id); // Changed to ServiceResult<bool>

        Task<ServiceResult<bool>> AddExerciseAsync(AddExerciseToWorkoutDto addExerciseToWorkoutDto); // Changed to ServiceResult<bool>

        Task<ServiceResult<bool>> RemoveExerciseFromWorkoutAsync(Guid workoutId, Guid exerciseId); // Changed to ServiceResult<bool>

        Task<ServiceResult<bool>> UpdateSetAsync(UpdateSetDto dto); // Changed to ServiceResult<bool>
        Task<ServiceResult<bool>> DeleteSetAsync(Guid setId); // Changed to ServiceResult<bool>

    }
}
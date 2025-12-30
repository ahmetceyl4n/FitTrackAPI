using FitnessApp.Application.Common.Models; // Added
using FitnessApp.Application.DTOs.WorkoutDTOs;

namespace FitnessApp.Application.Services
{
    public interface IWorkoutService
    {
        Task<ServiceResult<List<WorkoutDto>>> GetAllAsync();
        Task<ServiceResult<WorkoutDto>> GetByIdAsync(Guid id);
        Task<ServiceResult<Guid>> CreateAsync(CreateWorkoutDto createDto);
        Task<ServiceResult<bool>> UpdateAsync(UpdateWorkoutDto updateDto); 
        Task<ServiceResult<bool>> DeleteAsync(Guid id); 

        Task<ServiceResult<bool>> AddExerciseAsync(AddExerciseToWorkoutDto addExerciseToWorkoutDto); 

        Task<ServiceResult<bool>> RemoveExerciseFromWorkoutAsync(Guid workoutId, Guid exerciseId); 

        Task<ServiceResult<bool>> UpdateSetAsync(UpdateSetDto dto); 
        Task<ServiceResult<bool>> DeleteSetAsync(Guid setId); 

    }
}
using FitnessApp.Application.DTOs.WorkoutDTOs;

namespace FitnessApp.Application.Services
{
    public interface IWorkoutService
    {
        Task<List<WorkoutDto>> GetAllAsync();
        Task<WorkoutDto> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(CreateWorkoutDto createDto);
        Task UpdateAsync(UpdateWorkoutDto updateDto);
        Task DeleteAsync(Guid id);

        Task AddExerciseAsync(AddExerciseToWorkoutDto addExerciseToWorkoutDto);

    }
}
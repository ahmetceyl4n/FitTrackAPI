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

        // Egzersiz ekleme/çıkarma işlemleri de o antrenmanın sahibi tarafından mı yapılıyor kontrol edilmeli
        Task<ServiceResult<bool>> AddExerciseAsync(AddExerciseToWorkoutDto addExerciseToWorkoutDto); 

        Task<ServiceResult<bool>> RemoveExerciseFromWorkoutAsync(Guid workoutId, Guid exerciseId); 

        // Set işlemleri için de dolaylı yoldan kontrol gerekebilir ama şimdilik doğrudan Set ID ile gidiyoruz.
        // İdeal dünyada Set -> WorkoutExercise -> Workout -> UserId zinciri kontrol edilmeli.
        // Başlangıç için yukarıdakiler yeterli.
        Task<ServiceResult<bool>> UpdateSetAsync(UpdateSetDto dto); 
        Task<ServiceResult<bool>> DeleteSetAsync(Guid setId); 

    }
}
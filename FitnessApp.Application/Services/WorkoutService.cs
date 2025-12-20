using AutoMapper;
using FitnessApp.Application.Common.Interfaces;
using FitnessApp.Application.DTOs.WorkoutDTOs;
using FitnessApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Application.Services
{
    class WorkoutService : IWorkoutService
    {
        private readonly IGenericRepository<Workout> _workoutRepository;
        private readonly IGenericRepository<Exercise> _exerciseRepository;
        private readonly IGenericRepository<WorkoutExercise> _workoutExerciseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorkoutService(IMapper mapper, IUnitOfWork unitOfWork, IGenericRepository<Workout> workoutRepository, IGenericRepository<Exercise> exerciseRepository, IGenericRepository<WorkoutExercise> workoutExerciseRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _workoutRepository = workoutRepository;
            _exerciseRepository = exerciseRepository;
            _workoutExerciseRepository = workoutExerciseRepository;
        }

        public async Task AddExerciseAsync(AddExerciseToWorkoutDto dto)
        {
            var workout = await _workoutRepository.GetByIdAsync(dto.WorkoutId);
            if (workout == null) throw new Exception("Antrenman bulunamadı!");

            var exercise = await _exerciseRepository.GetByIdAsync(dto.ExerciseId);
            if (exercise == null) throw new Exception("Egzersiz bulunamadı!");

            var workoutExercise = _mapper.Map<WorkoutExercise>(dto);

            await _workoutExerciseRepository.AddAsync(workoutExercise);
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task<Guid> CreateAsync(CreateWorkoutDto createDto)
        {
            createDto.Date = createDto.Date.ToUniversalTime();

            var workout = _mapper.Map<Workout>(createDto);

            await _workoutRepository.AddAsync(workout);
            await _unitOfWork.SaveChangesAsync();
            return workout.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            var workout = await _workoutRepository.GetByIdAsync(id);
            if (workout != null)
            {
                _workoutRepository.Remove(workout);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<List<WorkoutDto>> GetAllAsync()
        {
            var workouts = await _workoutRepository.GetAll().ToListAsync();
            return _mapper.Map<List<WorkoutDto>>(workouts);
        }

        public async Task<WorkoutDto> GetByIdAsync(Guid id)
        {
            var workout = await _workoutRepository.GetAll() // Sorguyu başlat
                .Include(w => w.WorkoutExercises)           // 1. Ara tabloyu (WorkoutExercise) dahil et
                    .ThenInclude(we => we.Exercise)         // 2. Oradan Egzersiz ismine (Exercise) zıpla
                .Include(w => w.WorkoutExercises)           // Tekrar ara tabloya dön
                    .ThenInclude(we => we.Sets)             // 3. Oradan Setlere (Sets) zıpla
                .FirstOrDefaultAsync(w => w.Id == id);      // 4. Ve ID'si eşleşen İLK kaydı getir.

            if (workout == null) return null;

            return _mapper.Map<WorkoutDto>(workout);
        }

        public async Task UpdateAsync(UpdateWorkoutDto updateDto)
        {
            var workout = await _workoutRepository.GetByIdAsync(updateDto.Id);
            if (workout != null) 
            {
                updateDto.Date = updateDto.Date.ToUniversalTime();

                _mapper.Map(updateDto, workout);
                _workoutRepository.Update(workout);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}

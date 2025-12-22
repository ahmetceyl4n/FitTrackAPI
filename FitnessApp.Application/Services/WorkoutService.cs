using AutoMapper;
using FitnessApp.Application.Common.Interfaces;
using FitnessApp.Application.DTOs.WorkoutDTOs;
using FitnessApp.Domain.Entities;
using FitnessApp.Application.Common.Models; // Added
using FluentValidation;
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
        private readonly IGenericRepository<Set> _setRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        IValidator<CreateWorkoutDto> _createWorkoutValidator;
        IValidator<AddExerciseToWorkoutDto> _addExerciseToWorkoutValidator;

        public WorkoutService(IMapper mapper, IUnitOfWork unitOfWork, IGenericRepository<Workout> workoutRepository, IGenericRepository<Exercise> exerciseRepository, IGenericRepository<WorkoutExercise> workoutExerciseRepository, IGenericRepository<Set> setRepository, IValidator<CreateWorkoutDto> createWorkoutValidator, IValidator<AddExerciseToWorkoutDto> addExerciseToWorkoutValidator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _workoutRepository = workoutRepository;
            _exerciseRepository = exerciseRepository;
            _workoutExerciseRepository = workoutExerciseRepository;
            _setRepository = setRepository;
            _createWorkoutValidator = createWorkoutValidator;
            _addExerciseToWorkoutValidator = addExerciseToWorkoutValidator;
        }

        public async Task<ServiceResult<bool>> AddExerciseAsync(AddExerciseToWorkoutDto dto)
        {
            var validationResult = await _addExerciseToWorkoutValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) 
            {
                // Validation hatasını düzgünce dönüyoruz
                return ServiceResult<bool>.Failure(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var workout = await _workoutRepository.GetByIdAsync(dto.WorkoutId);
            if (workout == null) return ServiceResult<bool>.Failure("Antrenman bulunamadı!");

            var exercise = await _exerciseRepository.GetByIdAsync(dto.ExerciseId);
            if (exercise == null) return ServiceResult<bool>.Failure("Egzersiz bulunamadı!");

            var workoutExercise = _mapper.Map<WorkoutExercise>(dto);

            await _workoutExerciseRepository.AddAsync(workoutExercise);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<Guid>> CreateAsync(CreateWorkoutDto createDto)
        {
            var validationResult = await _createWorkoutValidator.ValidateAsync(createDto);

            if (!validationResult.IsValid)
            {
                return ServiceResult<Guid>.Failure(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            createDto.Date = createDto.Date.ToUniversalTime();

            var workout = _mapper.Map<Workout>(createDto);

            await _workoutRepository.AddAsync(workout);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<Guid>.Success(workout.Id);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(Guid id)
        {
            var workout = await _workoutRepository.GetAll()
                .Include(w => w.WorkoutExercises)
                .FirstOrDefaultAsync(x => x.Id == id );
            
            if (workout == null)
            {
                return ServiceResult<bool>.Failure("Antrenman bulunamadı.");
            }

            foreach (var exerciseLink in workout.WorkoutExercises)
            {
                _workoutExerciseRepository.Remove(exerciseLink);
            }

            _workoutRepository.Remove(workout);

            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> DeleteSetAsync(Guid setId)
        {
            var set = await _setRepository.GetByIdAsync(setId);

            if (set == null) 
            {
                return ServiceResult<bool>.Failure("Set bulunamadı.");
            }
            
            _setRepository.Remove(set);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<List<WorkoutDto>>> GetAllAsync()
        {
            var workouts = await _workoutRepository.GetAll().ToListAsync();
            return ServiceResult<List<WorkoutDto>>.Success(_mapper.Map<List<WorkoutDto>>(workouts));
        }

        public async Task<ServiceResult<WorkoutDto>> GetByIdAsync(Guid id)
        {
            var workout = await _workoutRepository.GetAll() // Sorguyu başlat
                .Include(w => w.WorkoutExercises)           // 1. Ara tabloyu (WorkoutExercise) dahil et
                    .ThenInclude(we => we.Exercise)         // 2. Oradan Egzersiz ismine (Exercise) zıpla
                .Include(w => w.WorkoutExercises)           // Tekrar ara tabloya dön
                    .ThenInclude(we => we.Sets)             // 3. Oradan Setlere (Sets) zıpla
                .FirstOrDefaultAsync(w => w.Id == id);      // 4. Ve ID'si eşleşen İLK kaydı getir.

            if (workout == null) return ServiceResult<WorkoutDto>.Failure("Antrenman bulunamadı.");

            return ServiceResult<WorkoutDto>.Success(_mapper.Map<WorkoutDto>(workout));
        }

        public async Task<ServiceResult<bool>> RemoveExerciseFromWorkoutAsync(Guid workoutId, Guid exerciseId)
        {
            var record = await _workoutExerciseRepository.GetAll()
                .FirstOrDefaultAsync(x => x.WorkoutId == workoutId && x.ExerciseId == exerciseId);

            if (record == null)
            {
                return ServiceResult<bool>.Failure("Böyle bir kayıt bulunamadı! Zaten silinmiş olabilir.");
            }
            _workoutExerciseRepository.Remove(record);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> UpdateAsync(UpdateWorkoutDto updateDto)
        {
            var workout = await _workoutRepository.GetByIdAsync(updateDto.Id);
            if (workout == null) return ServiceResult<bool>.Failure("Antrenman bulunamadı.");

            updateDto.Date = updateDto.Date.ToUniversalTime();

            _mapper.Map(updateDto, workout);
            _workoutRepository.Update(workout);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> UpdateSetAsync(UpdateSetDto dto)
        {
            var set = await _setRepository.GetByIdAsync(dto.Id);
            if (set == null) return ServiceResult<bool>.Failure("Set Bulunamadı.");

            _mapper.Map(dto, set);

            _setRepository.Update(set);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<bool>.Success(true);
        }
    }
}

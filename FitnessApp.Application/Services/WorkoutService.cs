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
        private readonly ICurrentUserService _currentUserService; // Inject ediyoruz
        IValidator<CreateWorkoutDto> _createWorkoutValidator;
        IValidator<AddExerciseToWorkoutDto> _addExerciseToWorkoutValidator;

        public WorkoutService(IMapper mapper, IUnitOfWork unitOfWork, IGenericRepository<Workout> workoutRepository, IGenericRepository<Exercise> exerciseRepository, IGenericRepository<WorkoutExercise> workoutExerciseRepository, IGenericRepository<Set> setRepository, IValidator<CreateWorkoutDto> createWorkoutValidator, IValidator<AddExerciseToWorkoutDto> addExerciseToWorkoutValidator, ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _workoutRepository = workoutRepository;
            _exerciseRepository = exerciseRepository;
            _workoutExerciseRepository = workoutExerciseRepository;
            _setRepository = setRepository;
            _createWorkoutValidator = createWorkoutValidator;
            _addExerciseToWorkoutValidator = addExerciseToWorkoutValidator;
            _currentUserService = currentUserService;
        }

        // Helper property to get UserId easily
        private string UserId => _currentUserService.UserId;

        public async Task<ServiceResult<bool>> AddExerciseAsync(AddExerciseToWorkoutDto dto)
        {
            var validationResult = await _addExerciseToWorkoutValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) 
            {
                return ServiceResult<bool>.Failure(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            // Antrenmanı çekerken UserId'sini de kontrol ediyoruz
            var workout = await _workoutRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.WorkoutId && x.UserId == UserId);
            
            if (workout == null) return ServiceResult<bool>.Failure("Antrenman bulunamadı veya erişim yetkiniz yok!");

            var exercise = await _exerciseRepository.GetByIdAsync(dto.ExerciseId);
            if (exercise == null) return ServiceResult<bool>.Failure("Egzersiz bulunamadı!");

            var workoutExercise = _mapper.Map<WorkoutExercise>(dto);

            await _workoutExerciseRepository.AddAsync(workoutExercise);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<Guid>> CreateAsync(CreateWorkoutDto createDto)
        {
            try 
            {
                var validationResult = await _createWorkoutValidator.ValidateAsync(createDto);

                if (!validationResult.IsValid)
                {
                    return ServiceResult<Guid>.Failure(validationResult.Errors.Select(e => e.ErrorMessage));
                }

                if (string.IsNullOrEmpty(UserId))
                {
                     return ServiceResult<Guid>.Failure("Kullanıcı kimliği doğrulanamadı.");
                }

                createDto.Date = createDto.Date.ToUniversalTime();

                var workout = _mapper.Map<Workout>(createDto);
                
                // Kullanıcı ID'sini atıyoruz
                workout.UserId = UserId; 

                await _workoutRepository.AddAsync(workout);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResult<Guid>.Success(workout.Id);
            }
            catch (Exception ex)
            {
                // DEBUG: Hata detayını dönüyoruz
                return ServiceResult<Guid>.Failure($"Sunucu hatası: {ex.Message} | Inner: {ex.InnerException?.Message}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteAsync(Guid id)
        {
            var workout = await _workoutRepository.GetAll()
                .Include(w => w.WorkoutExercises)
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == UserId); // UserId Kontrolü
            
            if (workout == null)
            {
                return ServiceResult<bool>.Failure("Antrenman bulunamadı veya erişim yetkiniz yok.");
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
             // TODO: Burada da set'in bağlı olduğu workout'un sahibini kontrol etmek gerekebilir.
             // Şimdilik basit bırakıyoruz.
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
            // Sadece bu kullacının antrenmanlarını getir
            var workouts = await _workoutRepository.GetAll()
                                                   .Where(x => x.UserId == UserId)
                                                   .ToListAsync();
            return ServiceResult<List<WorkoutDto>>.Success(_mapper.Map<List<WorkoutDto>>(workouts));
        }

        public async Task<ServiceResult<WorkoutDto>> GetByIdAsync(Guid id)
        {
            var workout = await _workoutRepository.GetAll() 
                .Include(w => w.WorkoutExercises)           
                    .ThenInclude(we => we.Exercise)         
                .Include(w => w.WorkoutExercises)           
                    .ThenInclude(we => we.Sets)             
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == UserId); // UserId Kontrolü

            if (workout == null) return ServiceResult<WorkoutDto>.Failure("Antrenman bulunamadı veya erişim yetkiniz yok.");

            return ServiceResult<WorkoutDto>.Success(_mapper.Map<WorkoutDto>(workout));
        }

        public async Task<ServiceResult<bool>> RemoveExerciseFromWorkoutAsync(Guid workoutId, Guid exerciseId)
        {
            // Önce antrenmanın sahibini kontrol etmemiz lazım
            var workout = await _workoutRepository.GetAll().FirstOrDefaultAsync(x => x.Id == workoutId && x.UserId == UserId);
            if (workout == null) return ServiceResult<bool>.Failure("Antrenman bulunamadı veya erişim yetkiniz yok!");


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
            // UserId kontrolü
            var workout = await _workoutRepository.GetAll().FirstOrDefaultAsync(x => x.Id == updateDto.Id && x.UserId == UserId);
            
            if (workout == null) return ServiceResult<bool>.Failure("Antrenman bulunamadı veya erişim yetkiniz yok.");

            updateDto.Date = updateDto.Date.ToUniversalTime();

            _mapper.Map(updateDto, workout);
            _workoutRepository.Update(workout);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> UpdateSetAsync(UpdateSetDto dto)
        {
             // TODO: UserId kontrolü buraya da eklenebilir.
            var set = await _setRepository.GetByIdAsync(dto.Id);
            if (set == null) return ServiceResult<bool>.Failure("Set Bulunamadı.");

            _mapper.Map(dto, set);

            _setRepository.Update(set);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<bool>.Success(true);
        }
    }
}

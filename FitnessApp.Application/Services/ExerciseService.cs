using AutoMapper;
using FitnessApp.Application.Common.Interfaces;
using FitnessApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Application.DTOs.ExerciseDTOs;

using FitnessApp.Application.Common.Models; // Added

namespace FitnessApp.Application.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IGenericRepository<Exercise> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExerciseService(IUnitOfWork unitOfWork, IMapper mapper, IGenericRepository<Exercise> repository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<Guid>> CreateAsync(CreateExerciseDto createDto)
        {
            try
            {
                var exercise = _mapper.Map<Exercise>(createDto);
                await _repository.AddAsync(exercise);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResult<Guid>.Success(exercise.Id);
            }
            catch (Exception ex)
            {
                return ServiceResult<Guid>.Failure($"Egzersiz oluşturulurken bir hata oluştu: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteAsync(Guid id)
        {
            var exercise = await _repository.GetByIdAsync(id);
            if (exercise == null)
            {
                return ServiceResult<bool>.Failure("Egzersiz bulunamadı.");
            }

            _repository.Remove(exercise);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<List<ExerciseDto>>> GetAllAsync()
        {
            var exercisesQuery = _repository.GetAll();
            var exercises = await exercisesQuery.ToListAsync();
            var dtos = _mapper.Map<List<ExerciseDto>>(exercises);
            return ServiceResult<List<ExerciseDto>>.Success(dtos);
        }

        public async Task<ServiceResult<ExerciseDto>> GetByIdAsync(Guid id)
        {
            var exercise = await _repository.GetByIdAsync(id);
            if (exercise == null)
            {
                return ServiceResult<ExerciseDto>.Failure("Egzersiz bulunamadı.");
            }
            
            var dto = _mapper.Map<ExerciseDto>(exercise);
            return ServiceResult<ExerciseDto>.Success(dto);
        }

        public async Task<ServiceResult<bool>> UpdateAsync(UpdateExerciseDto updateDto)
        {
            var exercise = await _repository.GetByIdAsync(updateDto.Id);

            if (exercise == null)
            {
                return ServiceResult<bool>.Failure("Egzersiz bulunamadı.");
            }

            _mapper.Map(updateDto, exercise);
            _repository.Update(exercise);
            await _unitOfWork.SaveChangesAsync();
            
            return ServiceResult<bool>.Success(true);
        }
    }
}

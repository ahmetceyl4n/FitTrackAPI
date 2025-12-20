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

        public async Task<Guid> CreateAsync(CreateExerciseDto createDto)
        {
            var exercise = _mapper.Map<Exercise>(createDto);
            await _repository.AddAsync(exercise);
            await _unitOfWork.SaveChangesAsync();
            return exercise.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            var exercise = await _repository.GetByIdAsync(id);
            if (exercise != null) 
            {
                _repository.Remove(exercise);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<List<ExerciseDto>> GetAllAsync()
        {
            var exercisesQuery = _repository.GetAll();
            var exercises = await exercisesQuery.ToListAsync();
            return _mapper.Map<List<ExerciseDto>>(exercises);
        }

        public async Task<ExerciseDto> GetByIdAsync(Guid id)
        {
            var exercise = await _repository.GetByIdAsync(id);
            
            return _mapper.Map<ExerciseDto>(exercise);
           
        }

        public async Task UpdateAsync(UpdateExerciseDto updateDto)
        {
            var exercise = await _repository.GetByIdAsync(updateDto.Id);

            if (exercise != null) 
            {
                _mapper.Map(updateDto, exercise);

                _repository.Update(exercise);

                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}

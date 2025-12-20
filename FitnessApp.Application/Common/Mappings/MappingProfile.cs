using AutoMapper;
using FitnessApp.Application.DTOs.ExerciseDTOs;
using FitnessApp.Application.DTOs.WorkoutDTOs;
using FitnessApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Application.Common.Mappings
{
    class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Exercise,ExerciseDto>().ReverseMap();

            CreateMap<CreateExerciseDto, Exercise>();

            CreateMap<UpdateExerciseDto, Exercise>();

            CreateMap<Domain.Entities.Workout, WorkoutDto>()
                .ForMember(dest => dest.Exercises, opt => opt.MapFrom(src => src.WorkoutExercises)) 
                .ReverseMap();

            CreateMap<CreateWorkoutDto, Workout>();

            CreateMap<UpdateWorkoutDto, Workout>();

            CreateMap<CreateSetDto, Domain.Entities.Set>();

            CreateMap<AddExerciseToWorkoutDto, Domain.Entities.WorkoutExercise>()
                .ForMember(dest => dest.Sets, opt => opt.MapFrom(src => src.Sets));

            CreateMap<Domain.Entities.Set, SetDto>();

            CreateMap<Domain.Entities.WorkoutExercise, WorkoutExerciseDetailDto>()
                .ForMember(dest => dest.ExerciseName, opt => opt.MapFrom(src => src.Exercise.Name)) 
                .ForMember(dest => dest.TargetMuscleGroup, opt => opt.MapFrom(src => src.Exercise.TargetMuscleGroup)); 

            CreateMap<Domain.Entities.Workout, WorkoutDto>()
                .ForMember(dest => dest.Exercises, opt => opt.MapFrom(src => src.WorkoutExercises)) 
                .ReverseMap();
        }
    }
}

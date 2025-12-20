using FitnessApp.Application.DTOs.WorkoutDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Application.Validators
{
    public class AddExerciseToWorkoutDtoValidator : AbstractValidator<AddExerciseToWorkoutDto>
    {
        public AddExerciseToWorkoutDtoValidator()
        {
            RuleFor(x => x.WorkoutId).NotEmpty().WithMessage("Antrenman ID boş olamaz.");
            RuleFor(x => x.ExerciseId).NotEmpty().WithMessage("Egzersiz ID boş olamaz.");

            RuleForEach(x => x.Sets).SetValidator(new CreateSetDtoValidator());

        }
    }
}

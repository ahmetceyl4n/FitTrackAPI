using FitnessApp.Application.DTOs.WorkoutDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Application.Validators
{
    public class CreateWorkoutDtoValidator : AbstractValidator<CreateWorkoutDto>
    {
        public CreateWorkoutDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Antrenman adı boş olamaz.")
                .MaximumLength(50).WithMessage("Antrenman adı 50 karakterden uzun olamaz.");


        }
    }
}

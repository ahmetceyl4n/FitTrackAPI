using FitnessApp.Application.DTOs.ExerciseDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Application.Validators
{
    public class CreateExerciseDtoValidator : AbstractValidator<CreateExerciseDto>
    {

        public CreateExerciseDtoValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Egzersiz adı boş olamaz!") 
                .NotNull().WithMessage("Egzersiz adı mutlaka gönderilmeli!") 
                .MaximumLength(50).WithMessage("Egzersiz adı 50 karakterden uzun olamaz!");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama alanı zorunludur.")
                .MinimumLength(10).WithMessage("Açıklama en az 10 karakter olmalıdır, biraz detay verin :)");

            
            RuleFor(x => x.TargetMuscleGroup)
                .NotEmpty().WithMessage("Hedef kas grubu boş geçilemez.");
        }
    }
}

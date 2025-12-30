using FluentValidation;
using FitnessApp.Application.DTOs.WorkoutDTOs;

namespace FitnessApp.Application.Validators
{
    public class UpdateWorkoutDtoValidator : AbstractValidator<UpdateWorkoutDto>
    {
        public UpdateWorkoutDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Antrenman adı boş olamaz.")
                .MaximumLength(100).WithMessage("Antrenman adı 100 karakterden uzun olamaz.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Tarih alanı zorunludur.");
        }
    }
}

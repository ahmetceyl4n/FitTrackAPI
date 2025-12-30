using FluentValidation;
using FitnessApp.Application.DTOs.WorkoutDTOs;

namespace FitnessApp.Application.Validators
{
    public class UpdateSetDtoValidator : AbstractValidator<UpdateSetDto>
    {
        public UpdateSetDtoValidator()
        {
            RuleFor(x => x.Reps)
                .GreaterThan(0).WithMessage("Tekrar sayısı 0'dan büyük olmalıdır.");

            RuleFor(x => x.Weight)
                .GreaterThanOrEqualTo(0).WithMessage("Ağırlık negatif olamaz.");
        }
    }
}

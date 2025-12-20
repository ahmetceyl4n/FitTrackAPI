using FitnessApp.Application.DTOs.WorkoutDTOs;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Application.Validators
{
    public class CreateSetDtoValidator : AbstractValidator<CreateSetDto>
    {
        public CreateSetDtoValidator()
        {
            RuleFor(x => x.Reps)
                .GreaterThan(0).WithMessage("Tekrar sayısı 0'dan büyük olmalıdır.");

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Ağırlık 0'dan büyük olmalıdır.")
                .LessThan(400).WithMessage("Abartma Yaprak 400 üstü ne ");
        }
    }
}

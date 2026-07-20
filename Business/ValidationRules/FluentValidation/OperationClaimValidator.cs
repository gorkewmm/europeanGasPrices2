using Core.Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class OperationClaimValidator : AbstractValidator<OperationClaim>
    {
        public OperationClaimValidator()
        {
            RuleFor(oc =>oc).NotNull().WithMessage("Operation claim nesnesi boş olamaz.");

            RuleFor(oc => oc.Name)
                .NotEmpty().WithMessage("Operation claim adı boş olamaz.")
                .MinimumLength(2).WithMessage("Operation claim adı en az 2 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Operation claim adı 100 karakterden uzun olamaz.");
        }
    }
}

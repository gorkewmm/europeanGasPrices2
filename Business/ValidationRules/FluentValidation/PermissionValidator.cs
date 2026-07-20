using Core.Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class PermissionValidator : AbstractValidator<Permission>
    {
        public PermissionValidator()
        {
            RuleFor(p => p).NotNull().WithMessage("Permission nesnesi boş olamaz.");

            // 2. Name alanı boş olamaz, en az 2, en fazla 100 karakter olabilir (DbContext ile uyumlu)
            RuleFor(p=> p.Name).NotNull()
                .NotEmpty().WithMessage("Permission adı boş olamaz.")
                .MinimumLength(2).WithMessage("Yetki adı en az 2 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Yetki adı 100 karakterden uzun olamaz.");

            // 3. Description alanı opsiyoneldir ancak doldurulursa 250 karakteri geçemez
            RuleFor(p => p.Description)
                .MaximumLength(250).WithMessage("Açıklama 250 karakterden uzun olamaz.");
        }
    }
}

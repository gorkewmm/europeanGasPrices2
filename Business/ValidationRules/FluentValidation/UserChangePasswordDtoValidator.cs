using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class UserChangePasswordDtoValidator : AbstractValidator<UserForChangePasswordDto>
    {
        public UserChangePasswordDtoValidator()
        {
            RuleFor(u => u)
                .NotNull().WithMessage("Geçersiz şifre değiştirme verisi.");

            RuleFor(u => u.Id)
                .GreaterThan(0).WithMessage("Geçerli bir kullanıcı ID'si girilmelidir.");

            RuleFor(u => u.CurrentPassword)
                .NotEmpty().WithMessage("Mevcut şifre alanı boş olamaz.");

            RuleFor(u => u.NewPassword)
                .NotEmpty().WithMessage("Yeni şifre alanı boş olamaz.")
                .MinimumLength(6).WithMessage("Yeni şifre en az 6 karakter olmalıdır.");


            // İsteğe bağlı karmaşıklık kuralı ekleyebilirsin:
            // .Matches(@"[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.");
        }
    }
}

using Entities.DTOs.Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class UserUpdateDtoValidator : AbstractValidator<UserForUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(u => u)
                .NotNull().WithMessage("Geçersiz kullanıcı verisi.");

            RuleFor(u => u.Id)
                .GreaterThan(0).WithMessage("Geçerli bir kullanıcı ID'si girilmelidir.");

            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("Ad alanı boş olamaz.")
                .MaximumLength(50).WithMessage("Ad 50 karakterden uzun olamaz.");

            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("Soyad alanı boş olamaz.")
                .MaximumLength(50).WithMessage("Soyad 50 karakterden uzun olamaz.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("E-posta alanı boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
                .MaximumLength(100).WithMessage("E-posta 100 karakterden uzun olamaz.");

            RuleFor(u => u.NickName)
                .NotEmpty().WithMessage("Kullanıcı adı (NickName) boş olamaz.")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Kullanıcı adı 50 karakterden uzun olamaz.");
        }
    }
}

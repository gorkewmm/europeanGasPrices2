using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class UserForLoginDtoValidator : AbstractValidator<UserForLoginDto>
    {
        public UserForLoginDtoValidator()
        {
            RuleFor(u => u).NotEmpty().WithMessage("Kullanıcı verisi boş olamaz.");

            RuleFor(u => u.Email).NotEmpty().WithMessage("E-posta alanı boş olamaz.").
                    EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            RuleFor(u => u.Password).NotEmpty().WithMessage("Şifre alanı boş olamaz.");


        }
    }
}
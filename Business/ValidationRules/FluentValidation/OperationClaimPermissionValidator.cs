using Core.Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class OperationClaimPermissionValidator : AbstractValidator<OperationClaimPermission>
    {
        public OperationClaimPermissionValidator()
        {
            // 1. Nesnenin tamamen null gelme kontrolü
            RuleFor(ocp => ocp)
                .NotNull().WithMessage("Geçersiz rol-yetki verisi.");

            // 2. OperationClaimId kontrolü (0 veya negatif olamaz)
            RuleFor(ocp => ocp.OperationClaimId)
                .GreaterThan(0).WithMessage("Geçerli bir rol (OperationClaim) seçilmelidir.");

            // 3. PermissionId kontrolü (0 veya negatif olamaz)
            RuleFor(ocp => ocp.PermissionId)
                .GreaterThan(0).WithMessage("Geçerli bir yetki (Permission) seçilmelidir.");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;

namespace Core.Utilities.Security.Security
{
    // Bu attribute'ün hem sınıfların (Controller) hem de metotların (Action) üzerinde kullanılabilmesini sağlıyoruz.
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class SecuredOperation : Attribute, IAuthorizationFilter
    {
        private readonly string[] _requiredClaims;

        // Virgülle ayrılmış yetki veya rolleri kabul eder. Örn: [SecuredOperation("Admin,fuelprice.add")]
        public SecuredOperation(string rolesOrPermissions)
        {
            _requiredClaims = rolesOrPermissions.Split(',');
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // 1. Adım: Kullanıcı sisteme login olmuş mu? (Token'ı geçerli mi?)
            var user = context.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new ObjectResult("Sisteme giriş yapmanız gerekiyor.")
                {
                    StatusCode = 401 // Unauthorized (Kimliksiz Giriş)
                };
                return;
            }

            // 2. Adım: Kullanıcının token'ından gelen Roller (Role) ve Yetkileri (permission) çekiyoruz
            var userRoles = user.FindAll(ClaimTypes.Role).Select(c => c.Value);
            var userPermissions = user.FindAll("permission").Select(c => c.Value);

            // 3. Adım: Tüm bu rolleri ve yetkileri tek bir havuzda birleştiriyoruz
            var userClaims = userRoles.Concat(userPermissions).ToList();

            // 4. Adım: İstenen yetkilerden/rollerden EN AZ BİRİ kullanıcıda var mı?
            var hasClaim = _requiredClaims.Any(requiredClaim => userClaims.Contains(requiredClaim.Trim()));

            // 5. Adım: Eğer eşleşen hiçbir yetki yoksa geçişi engelle!
            if (!hasClaim)
            {
                context.Result = new ObjectResult("Bu işlem için yetkiniz bulunmuyor.")
                {
                    StatusCode = 403 // Forbidden (Yasaklı/Yetersiz Yetki)
                };
            }
        }
    }
}

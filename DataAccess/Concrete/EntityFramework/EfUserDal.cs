using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, PostgreContext>, IUserDal
    {
        public List<OperationClaim> GetClaims(User user)
        {
            using (var context = new PostgreContext())
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims
                             on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == user.Id
                             && userOperationClaim.IsDeleted == false // <-- EKLENEN KRİTİK FİLTRE
                             && operationClaim.IsDeleted == false
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
                return result.ToList();
            }
        }

        public List<Permission> GetPermissions(User user)
        {
            using (var context = new PostgreContext())
            {
                var result = from permission in context.Permissions
                             join ocp in context.OperationClaimPermissions
                                 on permission.Id equals ocp.PermissionId
                             join uoc in context.UserOperationClaims
                                 on ocp.OperationClaimId equals uoc.OperationClaimId
                             where uoc.UserId == user.Id
                                   && permission.IsDeleted == false
                                   && ocp.IsDeleted == false
                                   && uoc.IsDeleted == false
                             select new Permission
                             {
                                 Id = permission.Id,
                                 Name = permission.Name,
                                 Description = permission.Description
                             };

                // Mükerrer yetki tanımlarını (farklı rollerden gelen aynı yetkiler) teke düşürür
                return result.Distinct().ToList();
            }
        }
    }
}

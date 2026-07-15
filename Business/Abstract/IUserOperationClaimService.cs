using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IUserOperationClaimService
    {
        void Add(UserOperationClaim userOperationClaim);
        void Update(UserOperationClaim userOperationClaim);
        void Delete(UserOperationClaim userOperationClaim);
        UserOperationClaim GetById(int id);
        List<UserOperationClaim> GetAll();
        List<UserOperationClaim> GetByUserId(int userId); // Bir kullanıcının tüm rol eşleşmelerini getirmek için
    }
}

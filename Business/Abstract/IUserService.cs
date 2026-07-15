using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IUserService
    {
        void Add(User user);
        User GetByMail(string email);
        User GetByNickName(string nickName);
        List<OperationClaim> GetClaims(User user);
        void Update(User user); // Yeni
        void Delete(User user); // Yeni
        User GetById(int id);   // Yeni (Silme ve Güncelleme öncesi kontrol için şart)

        List<Permission> GetPermissions(User user);
    }
}

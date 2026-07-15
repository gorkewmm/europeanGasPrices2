using Business.Abstract;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public List<OperationClaim> GetClaims(User user)
        {
            return _userDal.GetClaims(user);
        }

        public void Add(User user)
        {
            _userDal.Add(user);
        }

        public User GetByMail(string email)
        {
            return _userDal.Get(u => u.Email == email);
        }

        public User GetByNickName(string nickName)
        {
            return _userDal.Get(u => u.NickName == nickName);
        }


        // YENİ: Kullanıcı bilgilerini güncelleme metodu
        public void Update(User user)
        {
            _userDal.Update(user);
        }

        // YENİ: Kullanıcı silme metodu (Repository'deki Soft-Delete'i tetikler)
        public void Delete(User user)
        {
            _userDal.Delete(user);
        }

        // YENİ: ID ile aktif kullanıcıyı bulma metodu
        public User GetById(int id)
        {
            return _userDal.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public List<Permission> GetPermissions(User user)
        {
            return _userDal.GetPermissions(user);
        }
    }
}

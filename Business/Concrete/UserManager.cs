using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.DTOs;
using Entities.DTOs.Entities.DTOs;
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

        public void Add(User user)
        {
            _userDal.Add(user);
        }
        public IResult Delete(int id)
        {
            var user = _userDal.Get(u => u.Id == id && !u.IsDeleted);
            if (user == null)
                return new ErrorResult("Silinecek kullanıcı bulunamadı.");

            _userDal.Delete(user);
            return new SuccessResult("Kullanıcı başarıyla silindi.");
        }

        public IDataResult<User> GetById(int id)
        {
            var user = _userDal.Get(u => u.Id == id && !u.IsDeleted);
            if (user == null)
                return new ErrorDataResult<User>("Kullanıcı bulunamadı.");

            return new SuccessDataResult<User>(user);
        }

        public IDataResult<User> GetByMail(string email)
        {
            var user = _userDal.Get(u => u.Email == email && !u.IsDeleted);
            if(user == null)
            {
                return new ErrorDataResult<User>("Kullanıcı bulunamadı.");
            }
            return new SuccessDataResult<User>(user);
        }

        public IDataResult<User> GetByNickName(string nickName)
        {
            var user = _userDal.Get(u => u.NickName == nickName && !u.IsDeleted);

            if (user == null)
            {
                return new ErrorDataResult<User>("Kullanıcı bulunamadı.");
            }
            return new SuccessDataResult<User>(user);

        }

        public List<OperationClaim> GetClaims(User user)
        {
            return _userDal.GetClaims(user);
        }

        public List<Permission> GetPermissions(User user)
        {
            return _userDal.GetPermissions(user);
        }

        public IResult Update(UserForUpdateDto userForUpdateDto)
        {
            var user = _userDal.Get(u => u.Id == userForUpdateDto.Id && !u.IsDeleted);

            if (user == null)
            {
                return new ErrorResult("Güncellenecek kullanıcı bulunamadı.");
            }

            user.FirstName = userForUpdateDto.FirstName;
            user.LastName = userForUpdateDto.LastName;
            user.Email = userForUpdateDto.Email;
            user.NickName = userForUpdateDto.NickName;

            _userDal.Update(user);
            return new SuccessResult("Kullanıcı başarıyla güncellendi.");
        }
        public IResult ChangePassword(UserForChangePasswordDto dto)
        {
            var user = _userDal.Get(u => u.Id == dto.Id && !u.IsDeleted);
            if (user == null)
                return new ErrorResult("Kullanıcı bulunamadı.");

            // Mevcut şifreyi HashingHelper ile doğrula
            if (!HashingHelper.VerifyPasswordHash(dto.CurrentPassword, user.PasswordHash, user.PasswordSalt))
                return new ErrorResult("Mevcut şifre hatalı.");

            // Yeni şifreyi hashle ve saltla
            byte[] newHash, newSalt;
            HashingHelper.CreatePasswordHash(dto.NewPassword, out newHash, out newSalt);

            user.PasswordHash = newHash;
            user.PasswordSalt = newSalt;

            _userDal.Update(user);
            return new SuccessResult("Şifre başarıyla güncellendi.");
        }

    }
}

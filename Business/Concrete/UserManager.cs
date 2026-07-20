using Business.Abstract;
using Business.ValidationRules.FluentValidation;
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
            // 1. FluentValidation Doğrulaması
            var validator = new UserUpdateDtoValidator();
            var validationResult = validator.Validate(userForUpdateDto);
            if (!validationResult.IsValid)
            {
                return new ErrorResult(validationResult.Errors[0].ErrorMessage);
            }

            // 2. İş Kuralları
            var user = _userDal.Get(u => u.Id == userForUpdateDto.Id && !u.IsDeleted);

            if (user == null)
            {
                return new ErrorResult("Güncellenecek kullanıcı bulunamadı.");
            }

            // E - posta çakışma kontrolü(Başka kullanıcı bu e-postayı kullanıyor mu?)
            var isEmailTaken = _userDal.Get(u => u.Id != userForUpdateDto.Id &&
                                                 u.Email.ToLower() == userForUpdateDto.Email.ToLower() &&
                                                 !u.IsDeleted);
            if (isEmailTaken != null)
            {
                return new ErrorResult("Bu e-posta adresi zaten kullanılıyor.");
            }

            // NickName çakışma kontrolü (Kullanıcının kendi NickName'i hariç başkası almış mı?)
            var isNickNameTaken = _userDal.Get(u => u.Id != userForUpdateDto.Id &&
                                                    u.NickName == userForUpdateDto.NickName &&
                                                    !u.IsDeleted);
            if (isNickNameTaken != null)
            {
                return new ErrorResult("Bu kullanıcı adı (NickName) zaten başka bir kullanıcı tarafından kullanılıyor.");
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
            // 1. FluentValidation Doğrulaması
            var validator = new UserChangePasswordDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return new ErrorResult(validationResult.Errors[0].ErrorMessage);
            }

            // 2. İş Kuralları
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

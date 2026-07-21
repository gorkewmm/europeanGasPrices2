using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {

            // 1. Validasyonu Manuel Çalıştır
            var validator = new UserForRegisterDtoValidator();
            var result = validator.Validate(userForRegisterDto);

            if (!result.IsValid)
            {
                // İlk bulduğu hatayı döner
                return new ErrorDataResult<User>(result.Errors[0].ErrorMessage);
            }

            // 1. E-posta kontrolü
            var userExists = UserExists(userForRegisterDto.Email);
            if (!userExists.Success)
            {
                return new ErrorDataResult<User>(userExists.Message);
            }

            // 2. NickName kontrolü
            var nickNameExists = NickNameExists(userForRegisterDto.NickName);
            if (!nickNameExists.Success)
            {
                return new ErrorDataResult<User>(nickNameExists.Message);
            }
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
                NickName = userForRegisterDto.NickName
            };
            _userService.Add(user);
            return new SuccessDataResult<User>(user, "Kullanıcı başarıyla kaydoldu.");
        }
        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            // 1. Validasyonu Manuel Çalıştır
            var validator = new UserForLoginDtoValidator();
            var result = validator.Validate(userForLoginDto);

            if (!result.IsValid)
            {
                return new ErrorDataResult<User>(result.Errors[0].ErrorMessage);
            }

            var dataResult = _userService.GetByMail(userForLoginDto.Email);
            if (!dataResult.Success)
            {
                return new ErrorDataResult<User>("Kullanıcı bulunamadı.");
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, dataResult.Data.PasswordHash, dataResult.Data.PasswordSalt))
            {
                return new ErrorDataResult<User>("Parola hatalı.");
            }
            return new SuccessDataResult<User>(dataResult.Data, "Giriş başarılı.");
        }
        public IResult UserExists(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new ErrorResult("e-mail boş olamaz");
            }
            if (_userService.GetByMail(email).Success)
            {
                return new ErrorResult("Kullanıcı zaten mevcut.");
            }
            return new SuccessResult();
        }

        public IResult NickNameExists(string nickName)
        {
            if (string.IsNullOrWhiteSpace(nickName))
            {
                return new ErrorResult("Kullanıcı adı boş olamaz");
            }
            if (_userService.GetByNickName(nickName).Success)
            {
                return new ErrorResult("Bu kullanıcı adı (NickName) zaten alınmış.");
            }
            return new SuccessResult();
        }
        
        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaims(user);
            var permissions = _userService.GetPermissions(user);

            var accessToken = _tokenHelper.CreateToken(user, claims, permissions);
            return new SuccessDataResult<AccessToken>(accessToken, "Token oluşturuldu.");
        }
    }
}

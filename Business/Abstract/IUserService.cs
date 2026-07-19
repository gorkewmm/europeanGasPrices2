using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.DTOs;
using Entities.DTOs.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IUserService
    {
        IDataResult<User> GetById(int id);
        IDataResult<User> GetByMail(string email);
        IDataResult<User> GetByNickName(string nickName);
        IResult Update(UserForUpdateDto userForUpdateDto);
        IResult Delete(int id);
        List<OperationClaim> GetClaims(User user);
        List<Permission> GetPermissions(User user);
        void Add(User user);  // Auth tarafı için kalabilir
        IResult ChangePassword(UserForChangePasswordDto dto);
    }
}

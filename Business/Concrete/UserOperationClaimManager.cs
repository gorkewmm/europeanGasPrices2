using Business.Abstract;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private readonly IUserOperationClaimDal _userOperationClaimDal;

        public UserOperationClaimManager(IUserOperationClaimDal userOperationClaimDal)
        {
            _userOperationClaimDal = userOperationClaimDal;
        }

        public void Add(UserOperationClaim userOperationClaim)
        {
            // İleride buraya "Aynı kullanıcıya aynı rol zaten tanımlanmış mı?" kontrolü (Business Rule) eklenebilir.
            _userOperationClaimDal.Add(userOperationClaim);
        }

        public void Update(UserOperationClaim userOperationClaim)
        {
            _userOperationClaimDal.Update(userOperationClaim);
        }

        public void Delete(UserOperationClaim userOperationClaim)
        {
            _userOperationClaimDal.Delete(userOperationClaim);
        }

        public UserOperationClaim GetById(int id)
        {
            return _userOperationClaimDal.Get(uoc => uoc.Id == id && uoc.IsDeleted == false);
        }

        public List<UserOperationClaim> GetAll()
        {
            return _userOperationClaimDal.GetAll(uoc => uoc.IsDeleted == false);
        }

        public List<UserOperationClaim> GetByUserId(int userId)
        {
            return _userOperationClaimDal.GetAll(uoc => uoc.UserId == userId && uoc.IsDeleted == false);
        }
    }
}

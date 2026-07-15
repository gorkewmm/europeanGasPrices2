using Business.Abstract;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class OperationClaimManager : IOperationClaimService
    {
        private readonly IOperationClaimDal _operationClaimDal;

        public OperationClaimManager(IOperationClaimDal operationClaimDal)
        {
            _operationClaimDal = operationClaimDal;
        }

        public void Add(OperationClaim operationClaim)
        {
            _operationClaimDal.Add(operationClaim);
        }

        public void Update(OperationClaim operationClaim)
        {
            _operationClaimDal.Update(operationClaim);
        }

        public void Delete(OperationClaim operationClaim)
        {
            _operationClaimDal.Delete(operationClaim);
        }

        public OperationClaim GetById(int id)
        {
            return _operationClaimDal.Get(oc => oc.Id == id && oc.IsDeleted == false);
        }

        public List<OperationClaim> GetAll()
        {
            return _operationClaimDal.GetAll(oc => oc.IsDeleted == false);
        }
    }
}

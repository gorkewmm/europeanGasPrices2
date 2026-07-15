using Business.Abstract;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class OperationClaimPermissionManager : IOperationClaimPermissionService
    {
        private readonly IOperationClaimPermissionDal _operationClaimPermissionDal;

        public OperationClaimPermissionManager(IOperationClaimPermissionDal operationClaimPermissionDal)
        {
            _operationClaimPermissionDal = operationClaimPermissionDal;
        }

        public void Add(OperationClaimPermission operationClaimPermission)
        {
            _operationClaimPermissionDal.Add(operationClaimPermission);
        }

        public void Update(OperationClaimPermission operationClaimPermission)
        {
            _operationClaimPermissionDal.Update(operationClaimPermission);
        }

        public void Delete(OperationClaimPermission operationClaimPermission)
        {
            _operationClaimPermissionDal.Delete(operationClaimPermission);
        }

        public OperationClaimPermission GetById(int id)
        {
            return _operationClaimPermissionDal.Get(o => o.Id == id && o.IsDeleted == false);
        }

        public List<OperationClaimPermission> GetAll()
        {
            return _operationClaimPermissionDal.GetAll(o => o.IsDeleted == false);
        }

        public List<OperationClaimPermission> GetByOperationClaimId(int operationClaimId)
        {
            return _operationClaimPermissionDal.GetAll(o => o.OperationClaimId == operationClaimId && o.IsDeleted == false);
        }
    }
}

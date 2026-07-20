using Core.Entities.Concrete;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IOperationClaimPermissionService
    {
        IResult Add(OperationClaimPermission operationClaimPermission);
        IResult Update(OperationClaimPermission operationClaimPermission);
        IResult Delete(int id);
        IDataResult<OperationClaimPermission> GetById(int id);
        IDataResult<List<OperationClaimPermission>> GetAll();
        IDataResult<List<OperationClaimPermission>> GetByOperationClaimId(int operationClaimId);
    }
}


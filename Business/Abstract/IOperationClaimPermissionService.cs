using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IOperationClaimPermissionService
    {
        void Add(OperationClaimPermission operationClaimPermission);
        void Update(OperationClaimPermission operationClaimPermission);
        void Delete(OperationClaimPermission operationClaimPermission);
        OperationClaimPermission GetById(int id);
        List<OperationClaimPermission> GetAll();

        // Ekstra: Bir role ait tüm yetkileri getirmek için
        List<OperationClaimPermission> GetByOperationClaimId(int operationClaimId);
    }
}

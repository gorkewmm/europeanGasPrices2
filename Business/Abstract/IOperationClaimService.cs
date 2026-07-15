using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IOperationClaimService
    {
        void Add(OperationClaim operationClaim);
        void Update(OperationClaim operationClaim);
        void Delete(OperationClaim operationClaim);
        OperationClaim GetById(int id);
        List<OperationClaim> GetAll();
    }
}

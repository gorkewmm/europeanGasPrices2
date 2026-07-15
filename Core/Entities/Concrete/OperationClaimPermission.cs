using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Concrete
{
    public class OperationClaimPermission : BaseEntity
    {
        public int OperationClaimId { get; set; }
        public int PermissionId { get; set; }
    }
}

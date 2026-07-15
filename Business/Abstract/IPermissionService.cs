using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IPermissionService
    {
        void Add(Permission permission);
        void Update(Permission permission);
        void Delete(Permission permission);
        Permission GetById(int id);
        List<Permission> GetAll();
    }
}

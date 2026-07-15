using Business.Abstract;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class PermissionManager : IPermissionService
    {
        private readonly IPermissionDal _permissionDal;

        public PermissionManager(IPermissionDal permissionDal)
        {
            _permissionDal = permissionDal;
        }

        public void Add(Permission permission)
        {
            _permissionDal.Add(permission);
        }

        public void Update(Permission permission)
        {
            _permissionDal.Update(permission);
        }

        public void Delete(Permission permission)
        {
            _permissionDal.Delete(permission);
        }

        public Permission GetById(int id)
        {
            return _permissionDal.Get(p => p.Id == id && p.IsDeleted == false);
        }

        public List<Permission> GetAll()
        {
            return _permissionDal.GetAll(p => p.IsDeleted == false);
        }
    }
}

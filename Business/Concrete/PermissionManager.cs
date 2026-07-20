using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Security;
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

        public IResult Add(Permission permission)
        {
            if (permission == null || string.IsNullOrWhiteSpace(permission.Name))
            {
                return new ErrorResult("Geçersiz yetki verisi veya yetki adı boş olamaz.");
            }

            var exists = _permissionDal.Get(p => p.Name.ToLower() == permission.Name.ToLower() && !p.IsDeleted);

            if (exists != null)
            {
                return new ErrorResult("Permission zaten bulunuyor.");
            }

            _permissionDal.Add(permission);
            return new SuccessResult("Permission eklendi.");
        }

        public IResult Delete(int id)
        {
            var result = GetById(id);
            if (!result.Success)
            {
                return new ErrorResult(result.Message);
            }

            _permissionDal.Delete(result.Data);
            return new SuccessResult("Permission başarıyla silindi.");
        }

        public IDataResult<List<Permission>> GetAll()
        {
            var result = _permissionDal.GetAll(p=> !p.IsDeleted);
            
            if(result == null || result.Count == 0) {
                return new ErrorDataResult<List<Permission>>("Permission bulunamadı.");
            }
            return new SuccessDataResult<List<Permission>>(result);
        }

        public IDataResult<Permission> GetById(int id)
        {
            var result = _permissionDal.Get(p => p.Id == id && !p.IsDeleted);
            if (result == null)
            {
                return new ErrorDataResult<Permission>("Permission bulunamadı.");
            }
            return new SuccessDataResult<Permission>(result, "Permission başarıyla getirildi.");
        }

        public IResult Update(Permission permission)
        {
            if(permission == null || string.IsNullOrWhiteSpace(permission.Name))
            {
                return new ErrorResult("Geçersiz yetki verisi veya yetki adı boş olamaz.");
            }

            var existingPermission = _permissionDal.Get(p => p.Id == permission.Id && !p.IsDeleted);
            if (existingPermission == null)
            {
                return new ErrorResult("Güncellenmek istenen yetki bulunamadı.");
            }

            if (existingPermission.Name.ToLower() != permission.Name.ToLower()) 
            {
                var isNameTaken = _permissionDal.Get(p =>
                    p.Id != permission.Id &&
                    p.Name.ToLower() == permission.Name.ToLower() &&
                    !p.IsDeleted) != null;

                if (isNameTaken)
                {
                    return new ErrorResult("Bu isimde başka bir yetki zaten mevcut.");
                }
            }

            existingPermission.Name = permission.Name;
            existingPermission.Description = permission.Description;

            _permissionDal.Update(existingPermission);
            return new SuccessResult("Yetki başarıyla güncellendi.");
        }
    }
}

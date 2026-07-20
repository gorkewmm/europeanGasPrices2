using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class OperationClaimPermissionManager : IOperationClaimPermissionService
    {
        private readonly IOperationClaimPermissionDal _operationClaimPermissionDal;

        public IResult Add(OperationClaimPermission operationClaimPermission)
        {
            if (operationClaimPermission == null)
            {
                return new ErrorResult("Geçersiz rol-yetki verisi.");
            }

            var exists = _operationClaimPermissionDal.Get(ocp => ocp.OperationClaimId == operationClaimPermission.OperationClaimId
            && ocp.PermissionId == operationClaimPermission.PermissionId && !ocp.IsDeleted);

            if (exists != null)
            {
                return new ErrorResult("Bu rol-yetki ilişkisi zaten mevcut.");
            }

            _operationClaimPermissionDal.Add(operationClaimPermission);
            return new SuccessResult("Rol-yetki ilişkisi başarıyla eklendi.");
        }

        public IResult Delete(int id)
        {
            var result = GetById(id);

            if (!result.Success)
            {
                return new ErrorResult(result.Message);
            }
            _operationClaimPermissionDal.Delete(result.Data);
            return new SuccessResult("Rol-yetki ilişkisi başarıyla silindi.");

        }

        public IDataResult<List<OperationClaimPermission>> GetAll()
        {
            var operationClaimPermissions = _operationClaimPermissionDal.GetAll(c => c.IsDeleted == false);

            return new SuccessDataResult<List<OperationClaimPermission>>(operationClaimPermissions);
        }

        public IDataResult<OperationClaimPermission> GetById(int id)
        {
            var result = _operationClaimPermissionDal.Get(ocp => ocp.Id == id && !ocp.IsDeleted);
            if (result == null)
            {
                return new ErrorDataResult<OperationClaimPermission>("Rol-yetki ilişkisi bulunamadı.");
            }
            return new SuccessDataResult<OperationClaimPermission>(result);
        }

        public IDataResult<List<OperationClaimPermission>> GetByOperationClaimId(int operationClaimId)
        {
            var result = _operationClaimPermissionDal.GetAll(ocp => ocp.OperationClaimId == operationClaimId && !ocp.IsDeleted);
            if (result == null || result.Count == 0)
            {
                return new ErrorDataResult<List<OperationClaimPermission>>("Bu rol için herhangi bir yetki bulunamadı.");
            }
            return new SuccessDataResult<List<OperationClaimPermission>>(result);
        }

        public IResult Update(OperationClaimPermission operationClaimPermission)
        {
            if (operationClaimPermission == null)
            {
                return new ErrorResult("Geçersiz rol-yetki verisi.");
            }

            var existingPermission = _operationClaimPermissionDal.Get(ocp => ocp.Id == operationClaimPermission.Id && !ocp.IsDeleted);
            if (existingPermission == null)
            {
                return new ErrorResult("Güncellenmek istenen rol-yetki kaydı bulunamadı.");
            }

            var isDuplicate = _operationClaimPermissionDal.Get(ocp =>
                ocp.Id != operationClaimPermission.Id &&
                ocp.OperationClaimId == operationClaimPermission.OperationClaimId &&
                ocp.PermissionId == operationClaimPermission.PermissionId &&
                !ocp.IsDeleted);

            if (isDuplicate != null)
            {
                return new ErrorResult("Bu rol-yetki ilişkisi zaten mevcut.");
            }

            _operationClaimPermissionDal.Update(operationClaimPermission);
            return new SuccessResult("Rol-yetki ilişkisi başarıyla güncellendi.");
        }
    }
}

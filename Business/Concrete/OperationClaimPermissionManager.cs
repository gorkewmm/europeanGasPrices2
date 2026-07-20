using Business.Abstract;
using Business.ValidationRules.FluentValidation;
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

        public OperationClaimPermissionManager(IOperationClaimPermissionDal operationClaimPermissionDal)
        {
            _operationClaimPermissionDal = operationClaimPermissionDal;
        }

        public IResult Add(OperationClaimPermission operationClaimPermission)
        {
            // 1. FluentValidation Doğrulaması
            var validator = new OperationClaimPermissionValidator();
            var validationResult = validator.Validate(operationClaimPermission);
            if (!validationResult.IsValid)
            {
                return new ErrorResult(validationResult.Errors[0].ErrorMessage);
            }

            // 2. İş Kuralları

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
            var result = _operationClaimPermissionDal.GetAll(c => c.IsDeleted == false);

            if (result == null || result.Count == 0)
            {
                return new ErrorDataResult<List<OperationClaimPermission>>("Sistemde listelenecek rol-yetki kaydı bulunamadı.");
            }

            return new SuccessDataResult<List<OperationClaimPermission>>(result);
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
            var result = _operationClaimPermissionDal.GetAll(ocp => ocp.OperationClaimId == operationClaimId && !ocp.IsDeleted)
                            ?? new List<OperationClaimPermission>();

            return new SuccessDataResult<List<OperationClaimPermission>>(result);
        }

        public IResult Update(OperationClaimPermission operationClaimPermission)
        {
            // 1. FluentValidation Doğrulaması
            var validator = new OperationClaimPermissionValidator();
            var validationResult = validator.Validate(operationClaimPermission);
            if (!validationResult.IsValid)
            {
                return new ErrorResult(validationResult.Errors[0].ErrorMessage);
            }

            // 2. İş Kuralları

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

            // Veritabanındaki nesneyi güvenli şekilde güncelleme
            existingPermission.OperationClaimId = operationClaimPermission.OperationClaimId;
            existingPermission.PermissionId = operationClaimPermission.PermissionId;

            _operationClaimPermissionDal.Update(existingPermission);
            return new SuccessResult("Rol-yetki ilişkisi başarıyla güncellendi.");
        }
    }
}

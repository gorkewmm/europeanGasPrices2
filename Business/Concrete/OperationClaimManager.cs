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
    public class OperationClaimManager : IOperationClaimService
    {
        private readonly IOperationClaimDal _operationClaimDal;

        public OperationClaimManager(IOperationClaimDal operationClaimDal)
        {
            _operationClaimDal = operationClaimDal;
        }

        public IResult Add(OperationClaim operationClaim)
        {
            if (operationClaim == null || string.IsNullOrWhiteSpace(operationClaim.Name))
            {
                return new ErrorResult("Rol adı boş olamaz.");
            }

            var existingOperationClaim = _operationClaimDal.Get(oc => oc.Name.ToLower() == operationClaim.Name.ToLower() && oc.IsDeleted == false);

            if (existingOperationClaim != null)
            {
                return new ErrorResult("Operation claim zaten bulunuyor.");
            }

            _operationClaimDal.Add(operationClaim);

            return new SuccessResult("Operation claim başarıyla eklendi.");
        }

        public IResult Delete(int id)
        {
            var result = GetById(id);

            if (!result.Success)
            {
                return new ErrorResult(result.Message);
            }
            _operationClaimDal.Delete(result.Data);
            return new SuccessResult("Operation claim başarıyla silindi.");
        }

        public IDataResult<List<OperationClaim>> GetAll()
        {
            var result = _operationClaimDal.GetAll(c => c.IsDeleted == false);

            if (result == null || result.Count == 0)
            {
                return new ErrorDataResult<List<OperationClaim>>("Sistemde listelenecek operation claim kaydı bulunamadı.");
            }


            return new SuccessDataResult<List<OperationClaim>>(result);
        }

        public IDataResult<OperationClaim> GetById(int id)
        {
            var operationClaim = _operationClaimDal.Get(c => c.Id == id && c.IsDeleted == false);

            if (operationClaim == null)
            {
                return new ErrorDataResult<OperationClaim>("Operation claim bulunamadı.");
            }

            return new SuccessDataResult<OperationClaim>(operationClaim);
        }

        public IResult Update(OperationClaim operationClaim)
        {
            if (operationClaim == null || string.IsNullOrWhiteSpace(operationClaim.Name))
            {
                return new ErrorResult("Operation claim adı boş olamaz.");
            }

            var existingOperationClaim = _operationClaimDal.Get(oc => oc.Id == operationClaim.Id && !oc.IsDeleted);

            if (existingOperationClaim == null)
            {
                return new ErrorResult("Güncellenecek Operation claim bulunamadı.");
            }

            if(existingOperationClaim.Name.ToLower() != operationClaim.Name.ToLower())
            {
                var isNameTaken = _operationClaimDal.
                    Get(oc => oc.Name.ToLower() == operationClaim.Name.ToLower() && oc.Id != operationClaim.Id && !oc.IsDeleted);
                
                if (isNameTaken != null)
                {
                    return new ErrorResult("Bu isimde başka bir Operation claim zaten mevcut.");
                }
            }

            existingOperationClaim.Name = operationClaim.Name;
            _operationClaimDal.Update(existingOperationClaim);

            return new SuccessResult("Rol başarıyla güncellendi.");

        }
    }
}
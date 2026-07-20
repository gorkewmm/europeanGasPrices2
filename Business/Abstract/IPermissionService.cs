using Core.Entities.Concrete;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IPermissionService
    {
        IResult Add(Permission permission);
        IResult Update(Permission permission);
        IResult Delete(int id);
        IDataResult<Permission> GetById(int id);
        IDataResult<List<Permission>> GetAll();
    }
}

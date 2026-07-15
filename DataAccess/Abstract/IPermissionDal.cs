using Core.DataAccess;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IPermissionDal : IEntityRepository<Permission>
    {
        // İleride sadece yetkilere özel karmaşık SQL/LINQ sorguları gerekirse buraya yazacağız.
        // Şimdilik CRUD operasyonlarını IEntityRepository'den miras alıyor.
    }
}
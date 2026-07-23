using Core.DataAccess;
using Entities.Concrete.Epdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract.Epdk
{
    public interface IEpdkFuelPriceDal : IEntityRepository<EpdkFuelPrice>
    {
    }
}

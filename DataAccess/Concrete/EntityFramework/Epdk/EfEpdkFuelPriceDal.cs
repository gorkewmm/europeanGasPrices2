using Core.DataAccess.EntityFramework;
using DataAccess.Abstract.Epdk;
using Entities.Concrete.Epdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework.Epdk
{
    public class EfEpdkFuelPriceDal : EfEntityRepositoryBase<EpdkFuelPrice,PostgreContext> , IEpdkFuelPriceDal
    {
    }
}

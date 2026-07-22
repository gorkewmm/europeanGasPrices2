using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IFuelPriceDal : IEntityRepository<FuelPrice>
    {
        decimal GetAverage(Expression<Func<FuelPrice,decimal>> keySelector);
        List<FuelPrice> GetTopByAscending(Expression<Func<FuelPrice,decimal>> keySelector, int topCount);
        List<FuelPrice> GetTopByDescending(Expression<Func<FuelPrice,decimal>> keySelector, int topCount);

        //Pagination
        PagedResultDto<FuelPrice> GetPaged(PageRequestDto pageRequest);
    }
}
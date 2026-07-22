using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfFuelPriceDal : EfEntityRepositoryBase<FuelPrice, PostgreContext>, IFuelPriceDal
    {
        public decimal GetAverage(Expression<Func<FuelPrice, decimal>> keySelector)
        {
            using (PostgreContext ctx = new PostgreContext())
            {
                return ctx.Set<FuelPrice>().Average(keySelector);
            }
        }

        public List<FuelPrice> GetTopByAscending(Expression<Func<FuelPrice, decimal>> keySelector, int topCount)
        {
            using (PostgreContext ctx = new PostgreContext())
            {
                return ctx.Set<FuelPrice>().OrderBy(keySelector).Take(topCount).ToList();
            }
        }

        public List<FuelPrice> GetTopByDescending(Expression<Func<FuelPrice, decimal>> keySelector, int topCount)
        {
            using (PostgreContext ctx = new PostgreContext())
            {
                return ctx.Set<FuelPrice>().OrderByDescending(keySelector).Take(topCount).ToList();
            }
        }


        // Pagination
        public PagedResultDto<FuelPrice> GetPaged(PageRequestDto pageRequest)
        {
            using (PostgreContext ctx = new PostgreContext())
            {
                //silinmeyen verileri al
                var query = ctx.Set<FuelPrice>().Where(x => !x.IsDeleted);

                var totalCount = query.Count();

                var items = query.OrderByDescending(x => x.PriceDate)
                    .Skip(pageRequest.PageIndex * pageRequest.PageSize)
                    .Take(pageRequest.PageSize)
                    .ToList();

                return new PagedResultDto<FuelPrice>
                {
                    Items = items,
                    PageIndex = pageRequest.PageIndex,
                    PageSize = pageRequest.PageSize,
                    TotalCount = totalCount
                };
            }
        }

    }
}
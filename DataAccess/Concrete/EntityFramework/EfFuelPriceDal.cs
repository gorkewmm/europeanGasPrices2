using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
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
    }
}
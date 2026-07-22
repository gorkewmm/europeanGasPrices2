using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Mappers
{
    public static class EpdkBultenMapper
    {
        public static FuelPrice ToTurkeyFuelPriceEntity(EpdkBultenResponseDto dto)
        {
            if (dto?.Data == null || dto.Data.Count == 0)
            {
                return null;
            }

            var benzinItem = dto.Data.FirstOrDefault(x =>
                x.Yakit != null && x.Yakit.Trim().ToUpper().Contains("BENZIN")
            );

            var motorinItem = dto.Data.FirstOrDefault(x =>
                x.Yakit != null && x.Yakit.Trim().ToUpper().Contains("MOTORIN")
            );

            return new FuelPrice
            {
                Country = "Turkey",
                Currency = "TRY",
                Gasoline = benzinItem != null ? benzinItem.Fiyat : 0,
                Diesel = motorinItem != null ? motorinItem.Fiyat : 0,
                Lpg = 0, 
                PriceDate = DateTime.Now
            };

        }
    }
}

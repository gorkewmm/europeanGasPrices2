using System.Globalization;
using Entities.Concrete;
using Entities.DTOs;

namespace Entities.Mappers
{
    public static class FuelPriceMapper
    {
        public static FuelPrice ToEntity(FuelPriceApiDto dto)
        {
            return new FuelPrice
            {
                Country = dto.Country,
                Currency = dto.Currency,
                Gasoline = ParseDecimal(dto.Gasoline),
                Diesel = ParseDecimal(dto.Diesel),
                Lpg = ParseDecimal(dto.Lpg),
                PriceDate = DateTime.Now
            };
        }
        public static FuelPrice ToEntity(FuelPriceCreateDto dto) 
        {
            return new FuelPrice
            {
                Country = dto.Country,
                Currency = dto.Currency,
                Gasoline = dto.Gasoline,
                Diesel = dto.Diesel,
                Lpg = dto.Lpg,
                PriceDate = DateTime.Now
            };
        }

        public static void UpdateEntity(
            FuelPrice entity,
            FuelPriceUpdateDto dto)
        {
            entity.Country = dto.Country;
            entity.Currency = dto.Currency;
            entity.Gasoline = dto.Gasoline;
            entity.Diesel = dto.Diesel;
            entity.Lpg = dto.Lpg;
        }

        private static decimal ParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "-")
            {
                return 0;
            }

            value = value.Replace(',', '.');

            if (decimal.TryParse(value,
                                 NumberStyles.Any,
                                 CultureInfo.InvariantCulture,
                                 out decimal result))
            {
                return result;
            }

            return 0;
        }
    }
}
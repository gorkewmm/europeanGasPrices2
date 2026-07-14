using System.Globalization;
using Entities.Concrete;
using Entities.DTOs;

namespace Entities.Mappers
{
    public static class FuelPriceMapper
    {
        public static FuelPrice ToEntity(FuelPriceDto dto)
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
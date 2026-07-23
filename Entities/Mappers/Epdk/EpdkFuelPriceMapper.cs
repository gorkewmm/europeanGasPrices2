using Entities.Concrete.Epdk;
using Entities.DTOs.Epdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Mappers.Epdk
{
    public static class EpdkFuelPriceMapper
    {
        public static List<EpdkFuelPrice> ToEntity(EpdkBultenResponseDto epdkResponse)
        {
            List<EpdkFuelPrice> list = [];

            if(epdkResponse.Data == null || epdkResponse.Data.Count == 0)
            {
                return list;
            }

            foreach (var data in epdkResponse.Data)
            {
                var epdkFuelPrice = new EpdkFuelPrice
                {
                    Fiyat = data.Fiyat,
                    Yakit = data.Yakit,
                    Tarih = data.Tarih,
                    OlcuBirimi = data.OlcuBirimi,
                    PriceDate = DateTime.Now
                };

                list.Add(epdkFuelPrice);
            }

            return list;
        }
    }
}

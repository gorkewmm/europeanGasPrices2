using Business.Abstract.Epdk;
using Core.Utilities.Results;
using DataAccess.Abstract.Epdk;
using Entities.Concrete.Epdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete.Epdk
{
    public class EpdkFuelPriceManager : IEpdkFuelPriceService
    {
        private readonly IEpdkFuelPriceDal _epdkFuelPriceDal;
        public EpdkFuelPriceManager(IEpdkFuelPriceDal epdkFuelPriceDal)
        {
            _epdkFuelPriceDal = epdkFuelPriceDal;
        }

        public IResult AddPriceHistoryIfChanged(List<EpdkFuelPrice> epdkFuelPrices)
        {
            if (epdkFuelPrices == null || epdkFuelPrices.Count == 0)
            {
                return new ErrorResult("Kaydedilecek EPDK veri listesi bulunamadı veya liste boş.");
            }

            int addedCount = 0;
            foreach (var item in epdkFuelPrices)
            {
                var lastRecord = _epdkFuelPriceDal
                    .GetAll(x => x.Yakit.ToLower() == item.Yakit.ToLower() && !x.IsDeleted)
                    .OrderByDescending(x => x.PriceDate)
                    .FirstOrDefault();

                // Eğer veritabanında daha önce bu yakıt yoksa 
                // VEYA son kaydedilen Fiyat/Bülten Tarihi (Tarih) değişmişse ekle
                if (lastRecord == null || lastRecord.Fiyat != item.Fiyat || lastRecord.Tarih != item.Tarih)
                {
                    _epdkFuelPriceDal.Add(item);
                    addedCount++;
                }
            }

            if (addedCount == 0)
            {
                return new SuccessResult("EPDK verilerinde herhangi bir fiyat veya bülten güncellemesi bulunmadığı için yeni kayıt eklenmedi.");
            }
            return new SuccessResult($"EPDK yakıt fiyatları başarıyla güncellendi. {addedCount} yeni fiyat kaydı işlendi.");

        }

        public IDataResult<List<EpdkFuelPrice>> GetAll()
        {
            var result = _epdkFuelPriceDal.GetAll(x => !x.IsDeleted);
            return new SuccessDataResult<List<EpdkFuelPrice>>(result);
        }
    }
}

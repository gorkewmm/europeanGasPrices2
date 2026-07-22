using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Business.Concrete
{
    public class FuelPriceManager : IFuelPriceService
    {
        private readonly IFuelPriceDal _fuelPriceDal;

        public FuelPriceManager(IFuelPriceDal fuelPriceDal)
        {
            _fuelPriceDal = fuelPriceDal;
        }

        public IDataResult<List<FuelPrice>> GetAll()
        {
            var result = _fuelPriceDal.GetAll();
            if (result.Count == 0)
            {
                return new ErrorDataResult<List<FuelPrice>>("Yakıt fiyatı bulunamadı");
            }
            return new SuccessDataResult<List<FuelPrice>>(result);
        }

        public IResult Update(FuelPriceUpdateDto fuelPriceUpdateDto)
        {
            var entity = _fuelPriceDal.Get(x => x.Id == fuelPriceUpdateDto.Id);
            if (entity == null)
            {
                return new ErrorResult("Yakıt fiyatı bulunamadı");
            }

            FuelPriceMapper.UpdateEntity(entity, fuelPriceUpdateDto);
            _fuelPriceDal.Update(entity);
            return new SuccessResult("Yakıt fiyatı güncellendi");
        }

        public IResult Delete(int id)
        {
            // 1. Önce silinecek veriyi id üzerinden veri tabanından buluyoruz
            var fuelPriceToDelete = _fuelPriceDal.Get(x => x.Id == id);

            // 2. Eğer böyle bir veri yoksa hata dönüyoruz (Güvenlik kontrolü)
            if (fuelPriceToDelete == null)
            {
                return new ErrorResult("Silinecek yakıt fiyatı bulunamadı.");
            }

            // 3. Veri varsa, mevcut repo metoduna gönderiyoruz (Senin yazdığın Remove metodu çalışacak)
            _fuelPriceDal.Delete(fuelPriceToDelete);

            return new SuccessResult("Yakıt fiyatı başarıyla silindi");
        }
        public IDataResult<decimal> GetAverageDieselPrice()
        {
            var result = _fuelPriceDal.GetAverage(x => x.Diesel);
            return new SuccessDataResult<decimal>(result);
        }

        public IDataResult<decimal> GetAverageGasolinePrice()
        {
            var result = _fuelPriceDal.GetAverage(x => x.Gasoline);
            return new SuccessDataResult<decimal>(result);
        }

        public IDataResult<decimal> GetAverageLpgPrice()
        {
            var result = _fuelPriceDal.GetAverage(x => x.Lpg);
            return new SuccessDataResult<decimal>(result);
        }

        public IDataResult<List<FuelPrice>> GetByCountry(string country)
        {
            var result = _fuelPriceDal.GetAll(x => x.Country == country);

            if (result.Count == 0)
            {
                return new ErrorDataResult<List<FuelPrice>>("Ülke bulunamadı");
            }
            return new SuccessDataResult<List<FuelPrice>>(result);
        }

        public IDataResult<List<FuelPrice>> GetByCurrency(string currency)
        {
            var result = _fuelPriceDal.GetAll(x => x.Currency == currency);

            if (result.Count == 0)
            {
                return new ErrorDataResult<List<FuelPrice>>("Para birimi bulunamadı");
            }
            return new SuccessDataResult<List<FuelPrice>>(result);
        }

        public IDataResult<FuelPrice> GetById(int id)
        {
            var result = _fuelPriceDal.Get(x => x.Id == id);

            if (result == null)
            {
                return new ErrorDataResult<FuelPrice>("Yakıt kaydı bulunamadı");
            }
            return new SuccessDataResult<FuelPrice>(result);
        }

        public IDataResult<string> GetCheapestFuelType(string country)
        {
            var fuel = _fuelPriceDal.GetAll(x => x.Country == country).OrderByDescending(x => x.PriceDate).FirstOrDefault();
            if (fuel == null)
            {
                return new ErrorDataResult<string>("Ülke bulunamadı");
            }

            string result;

            if (fuel.Gasoline <= fuel.Diesel && fuel.Gasoline <= fuel.Lpg)
            {
                result = "Gasoline";
            }

            else if (fuel.Diesel <= fuel.Gasoline && fuel.Diesel <= fuel.Lpg)
            {
                result = "Diesel";
            }

            else
            {
                result = "Lpg";
            }

            return new SuccessDataResult<string>(result);
        }

        public IDataResult<List<FuelPrice>> GetCheapestGasolineCountries(int count)
        {
            var result = _fuelPriceDal.GetTopByAscending(x => x.Gasoline, count);

            return new SuccessDataResult<List<FuelPrice>>(result);
        }


        public IDataResult<List<FuelPrice>> GetMostExpensiveGasolineCountries(int count)
        {
            var result = _fuelPriceDal.GetTopByDescending(x => x.Gasoline, count);

            return new SuccessDataResult<List<FuelPrice>>(result);
        }

        public IDataResult<List<FuelPrice>> GetCheapestDieselCountries(int count)
        {
            var result = _fuelPriceDal.GetTopByAscending(x=> x.Diesel, count);

            return new SuccessDataResult<List<FuelPrice>>(result);
        }

        public IDataResult<List<FuelPrice>> GetMostExpensiveDieselCountries(int count)
        {
            var result = _fuelPriceDal.GetTopByDescending(x => x.Diesel, count);

            return new SuccessDataResult<List<FuelPrice>>(result);
        }

        public IDataResult<List<FuelPrice>> GetCheapestLpgountries(int count)
        {
            var result = _fuelPriceDal.GetTopByAscending(x => x.Lpg, count);

            return new SuccessDataResult<List<FuelPrice>>(result);
        }

        public IDataResult<List<FuelPrice>> GetMostExpensiveLpgCountries(int count)
        {
            var result = _fuelPriceDal.GetAll().OrderByDescending(x => x.Lpg).Take(count).ToList();

            return new SuccessDataResult<List<FuelPrice>>(result);
        }
        public IDataResult<List<FuelPrice>> GetCountriesByGasolineRange(decimal min, decimal max)
        {
            var result = _fuelPriceDal.GetAll(x => x.Gasoline >= min && x.Gasoline <= max);

            return new SuccessDataResult<List<FuelPrice>>(result);
        }

        public IResult AddFromApi(List<FuelPriceApiDto> fuelPriceDtos)
        {
            if (fuelPriceDtos == null || fuelPriceDtos.Count == 0)
            {
                return new ErrorResult("Kaydedilecek veri bulunamadı.");
            }


            foreach (var dto in fuelPriceDtos)
            {
                //var current = _fuelPriceDal.Get(x =>
                //    x.Country == dto.Country);


                // Yeni kayıt

                var entity = FuelPriceMapper.ToEntity(dto);

                //entity.CreatedDate = DateTime.Now;
                //entity.UpdatedDate = DateTime.Now;

                _fuelPriceDal.Add(entity);



                // Mevcut kayıt güncelle
                //else
                //{
                //    var entity = FuelPriceMapper.ToEntity(dto);


                //    current.Currency = entity.Currency;
                //    current.Gasoline = entity.Gasoline;
                //    current.Diesel = entity.Diesel;
                //    current.Lpg = entity.Lpg;
                //    current.UpdatedDate = DateTime.Now;    
                //    current.PriceDate = DateTime.Now;

                //    _fuelPriceDal.Update(current);
                //}
            }

            return new SuccessResult("API verileri başarıyla aktarıldı.");
        }

        public IResult Add(FuelPriceCreateDto dto)
        {
            var entity = FuelPriceMapper.ToEntity(dto);

            _fuelPriceDal.Add(entity);

            return new SuccessResult("Yakıt fiyatı eklendi.");
        }

        public IDataResult<PagedResultDto<FuelPrice>> GetPaged(PageRequestDto pageRequest)
        {
            var result = _fuelPriceDal.GetPaged(pageRequest);
            return new SuccessDataResult<PagedResultDto<FuelPrice>>(result);
        }

        //public IResult AddFromApi(List<FuelPriceDto> fuelPriceDtos)
        //{
        //    if (fuelPriceDtos == null || fuelPriceDtos.Count == 0)
        //    {
        //        return new ErrorResult("Kaydedilecek veri bulunamadı");
        //    }

        //    // db de varsa güncelle, hiç yoksa onu ekle
        //    foreach (var dto in fuelPriceDtos)
        //    {
        //        var current = _fuelPriceDal.Get(x =>
        //            x.Country == dto.Country &&
        //            x.PriceDate == DateTime.UtcNow.Date);

        //        if (current == null)
        //        {
        //            _fuelPriceDal.Add(FuelPriceMapper.ToEntity(dto));
        //        }
        //        else
        //        {
        //            current.Gasoline = decimal.Parse(dto.Gasoline.Replace(',', '.'), CultureInfo.InvariantCulture);
        //            current.Diesel = decimal.Parse(dto.Diesel.Replace(',', '.'), CultureInfo.InvariantCulture);
        //            current.Lpg = decimal.Parse(dto.Lpg.Replace(',', '.'), CultureInfo.InvariantCulture);
        //            current.Currency = dto.Currency;
        //            current.UpdatedDate = DateTime.UtcNow;

        //            _fuelPriceDal.Update(current);
        //        }
        //    }


        //    return new SuccessResult("API yakıt verileri başarıyla kaydedildi");
        //}
    }
}

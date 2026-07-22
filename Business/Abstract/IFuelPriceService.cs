using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IFuelPriceService
    {
        IDataResult<List<FuelPrice>> GetAll();
        IDataResult<FuelPrice> GetById(int id);
        IResult Add(FuelPriceCreateDto dto);
        IResult Update(FuelPriceUpdateDto dto);
        IResult Delete(int id);

        //Ülkeye göre fiyat yakıtı getirme
        IDataResult<List<FuelPrice>> GetByCountry(string country);

        //En ucuz benzin fiyatına sahip ülkeleri getirme(ilk count adet ülke)
        IDataResult<List<FuelPrice>> GetCheapestGasolineCountries(int count);

        //En pahalı benzin fiyatına sahip ülkeleri getirme(ilk count adet ülke)
        IDataResult<List<FuelPrice>> GetMostExpensiveGasolineCountries(int count);


        //Dizeli en ucuz ülkeler
        IDataResult<List<FuelPrice>> GetCheapestDieselCountries(int count);
        //Dizeli en oahalı ülkeler
        IDataResult<List<FuelPrice>> GetMostExpensiveDieselCountries(int count);


        //Lpg en ucuz ülkeler
        IDataResult<List<FuelPrice>> GetCheapestLpgountries(int count);
        //Lpg en oahalı ülkeler
        IDataResult<List<FuelPrice>> GetMostExpensiveLpgCountries(int count);


        //Avrupa ortalama benzin fiyatı 
        IDataResult<decimal> GetAverageGasolinePrice();

        //Avrupa ortalama dizel fiyatı 
        IDataResult<decimal> GetAverageDieselPrice();

        //Avrupa ortalama lpg fiyatı 
        IDataResult<decimal> GetAverageLpgPrice();

        //En ucuz yakıt tipi bulma
        IDataResult<string> GetCheapestFuelType(string country);

        //Para birimine göre filtreleme
        IDataResult<List<FuelPrice>> GetByCurrency(string currency);


        //Fiyat aralığına göre ülkeler(benzini 1-1.5 arası olan ülkeler)
        IDataResult<List<FuelPrice>> GetCountriesByGasolineRange(decimal min, decimal max);


        IResult AddFromApi(List<FuelPriceApiDto> fuelPriceDtos);

        IResult Add(FuelPrice fuelPrice);

        //Pagination
        IDataResult<PagedResultDto<FuelPrice>> GetPaged(PageRequestDto pageRequest);

    }
}
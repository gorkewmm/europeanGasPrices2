using Core.Utilities.Results;
using Entities.DTOs;

namespace WebAPI.Service.Abstract
{
    public interface IFuelPriceApiService
    {
        Task<List<FuelPriceDto>> GetFuelPricesFromApi();
        Task<IResult> ImportAndSaveFuelPricesAsync();
    }
}

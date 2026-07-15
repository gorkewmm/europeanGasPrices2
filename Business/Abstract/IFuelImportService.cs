using Core.Utilities.Results;
using Entities.DTOs;

namespace WebAPI.Service.Abstract
{
    public interface IFuelImportService
    {
        Task<List<FuelPriceDto>> GetFuelPricesFromApi();
        Task<IResult> ImportAndSaveFuelPricesAsync();
    }
}
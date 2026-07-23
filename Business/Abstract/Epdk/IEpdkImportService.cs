using Core.Utilities.Results;
using Entities.DTOs;
using Entities.DTOs.Epdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract.Epdk
{
    public interface IEpdkImportService
    {
        Task<EpdkBultenResponseDto> GetEpdkFuelPricesFromApi(DateTime raporTarihi);
        Task<IResult> ImportAndSaveEpdkFuelPricesAsync(DateTime raporTarihi);
    }
}

using Core.Utilities.Results;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract.Epdk
{
    public interface IEpdkBultenImportService
    {
        Task<EpdkBultenResponseDto> GetBultenFromApiAsync(DateTime raporTarihi);
        
        Task<IResult> ImportAndSaveTurkeyFuelPricesAsync(DateTime raporTarihi);
    }
}

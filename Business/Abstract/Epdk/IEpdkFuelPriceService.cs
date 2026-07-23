using Core.Utilities.Results;
using Entities.Concrete.Epdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract.Epdk
{
    public interface IEpdkFuelPriceService
    {
        IResult AddPriceHistoryIfChanged(List<EpdkFuelPrice> epdkFuelPrices);

        IDataResult<List<EpdkFuelPrice>> GetAll();
    }
}

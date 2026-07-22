using Business.Abstract;
using Core.Utilities.Security.Security;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Service.Abstract;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelPriceController : ControllerBase
    {
        private readonly IFuelPriceService _fuelPriceService;
        //private readonly IFuelPriceApiService _fuelPriceApiService;

        public FuelPriceController(IFuelPriceService fuelPriceService)
        {
            _fuelPriceService = fuelPriceService;
            //_fuelPriceApiService = fuelPriceApiService;
        }

        [HttpGet("getall")]
        [SecuredOperation("admin,manager,visitor,fuelprice.getall")]
        public IActionResult GetAll()
        {
            var result = _fuelPriceService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        //Pagination için endpoint
        [HttpGet("getpaged")]
        public IActionResult GetPaged([FromQuery] PageRequestDto pageRequest)
        {
            var result = _fuelPriceService.GetPaged(pageRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        [SecuredOperation("fuelprice.create")]
        public IActionResult GetById(int id)
        {
            var result = _fuelPriceService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbycountry")]
        public IActionResult GetByCountry(string country)
        {
            var result = _fuelPriceService.GetByCountry(country);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbycurrency")]
        public IActionResult GetByCurrency(string currency)
        {
            var result = _fuelPriceService.GetByCurrency(currency);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("add")]
        [SecuredOperation("admin,manager,fuelprice.create")]
        public IActionResult Add(FuelPriceCreateDto fuelPriceCreateDto)
        {
            var result = _fuelPriceService.Add(fuelPriceCreateDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("update")]
        [SecuredOperation("admin,fuelprice.update")]
        public IActionResult Update(FuelPriceUpdateDto dto)
        {
            var result = _fuelPriceService.Update(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("delete/{id}")]
        [SecuredOperation("admin,fuelprice.update")]
        public IActionResult Delete(int id)
        {
            var result = _fuelPriceService.Delete(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // --- İstatistiki ve Ortalama Hesaplama Endpointleri ---

        [HttpGet("getaveragedieselprice")]
        public IActionResult GetAverageDieselPrice()
        {
            var result = _fuelPriceService.GetAverageDieselPrice();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getaveragegasolineprice")]
        public IActionResult GetAverageGasolinePrice()
        {
            var result = _fuelPriceService.GetAverageGasolinePrice();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getaveragelpgprice")]
        public IActionResult GetAverageLpgPrice()
        {
            var result = _fuelPriceService.GetAverageLpgPrice();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getcheapestfueltype")]
        public IActionResult GetCheapestFuelType(string country)
        {
            var result = _fuelPriceService.GetCheapestFuelType(country);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // --- Sıralama (Top Count) Endpointleri ---

        [HttpGet("getcheapestgasolinecountries")]
        public IActionResult GetCheapestGasolineCountries(int count)
        {
            var result = _fuelPriceService.GetCheapestGasolineCountries(count);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getmostexpensivegasolinecountries")]
        public IActionResult GetMostExpensiveGasolineCountries(int count)
        {
            var result = _fuelPriceService.GetMostExpensiveGasolineCountries(count);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getcheapestdieselcountries")]
        public IActionResult GetCheapestDieselCountries(int count)
        {
            var result = _fuelPriceService.GetCheapestDieselCountries(count);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getmostexpensivedieselcountries")]
        public IActionResult GetMostExpensiveDieselCountries(int count)
        {
            var result = _fuelPriceService.GetMostExpensiveDieselCountries(count);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getcheapestlpgcountries")]
        public IActionResult GetCheapestLpgCountries(int count)
        {
            // Manager içerisindeki "GetCheapestLpgountries" imla hatasına sadık kalınmıştır
            var result = _fuelPriceService.GetCheapestLpgountries(count);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getmostexpensivelpgcountries")]
        public IActionResult GetMostExpensiveLpgCountries(int count)
        {
            var result = _fuelPriceService.GetMostExpensiveLpgCountries(count);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getcountriesbygasolinerange")]
        public IActionResult GetCountriesByGasolineRange(decimal min, decimal max)
        {
            var result = _fuelPriceService.GetCountriesByGasolineRange(min, max);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // --- DTO ve Dış Kaynak (API) Endpointi ---


        //Burası FuelImportController'a taşındı. FuelPriceController artık sadece CRUD ve istatistiksel işlemler için kullanılacak.

        //[HttpPost("importfromapi")]
        //public async Task<IActionResult> ImporFromApi()
        //{
        //    var fuelPriceDtos = await _fuelPriceApiService.GetFuelPricesFromApi();

        //    if(fuelPriceDtos == null || fuelPriceDtos.Count == 0)
        //    {
        //        return BadRequest("API'den ver. alınamadı");
        //    }

        //    var result = _fuelPriceService.AddFromApi(fuelPriceDtos);

        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }

        //    return BadRequest(result);
        //}


    }
}

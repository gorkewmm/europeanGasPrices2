using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Service.Abstract;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelImportController : ControllerBase
    {
        private readonly IFuelPriceApiService _fuelPriceApiService;

        public FuelImportController(IFuelPriceApiService fuelPriceApiService)
        {
            _fuelPriceApiService = fuelPriceApiService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportFromApi()
        {
            // Bu metot artık arkada hem çekecek hem kaydedecek, Controller sadece sonucu bekler.
            var result = await _fuelPriceApiService.ImportAndSaveFuelPricesAsync();

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

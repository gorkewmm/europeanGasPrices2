using Core.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Service.Abstract;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelImportController : ControllerBase
    {
        private readonly IFuelImportService _fuelImportService;

        public FuelImportController(IFuelImportService fuelImportService)
        {
            _fuelImportService = fuelImportService;
        }


        // (Sadece Avrupa / CollectAPI) ilk çalışmam
        [HttpPost("import")]
        public async Task<IActionResult> ImportFromApi()
        {
            // Bu metot artık arkada hem çekecek hem kaydedecek, Controller sadece sonucu bekler.
            var result = await _fuelImportService.ImportAndSaveFuelPricesAsync();

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

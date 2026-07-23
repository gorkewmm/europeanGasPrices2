using Business.Abstract.Epdk;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Epdk
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpdkPricesController : ControllerBase
    {
        private readonly IEpdkImportService _epdkImportService;
        private readonly IEpdkFuelPriceService _epdkFuelPriceService;

        public EpdkPricesController(
            IEpdkImportService epdkImportService,
            IEpdkFuelPriceService epdkFuelPriceService)
        {
            _epdkImportService = epdkImportService;
            _epdkFuelPriceService = epdkFuelPriceService;
        }

        // Belirtilen tarihteki EPDK verilerini çeker ve veritabanına işler.
        // Tarih gönderilmezse bugünün tarihini baz alır.
        [HttpPost("import")]
        public async Task<IActionResult> ImportPrices([FromQuery] DateTime? raporTarihi)
        {
            var targetDate = raporTarihi ?? DateTime.Now;
            var result = await _epdkImportService.ImportAndSaveEpdkFuelPricesAsync(targetDate);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        // Veritabanındaki tüm yakıt fiyat geçmişini getirir.        
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _epdkFuelPriceService.GetAll();

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}

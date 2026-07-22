using Business.Abstract.Epdk;
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
        private readonly IEpdkBultenImportService _epdkBultenImportService;

        public FuelImportController(IFuelImportService fuelImportService, IEpdkBultenImportService epdkBultenImportService)
        {
            _fuelImportService = fuelImportService;
            _epdkBultenImportService = epdkBultenImportService;
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

        // Yeni controller metodum. Aynı anda hem avrupa hem de Türkiye/EPDK)
        [HttpPost("import-all")]
        public async Task<IActionResult> ImportAllFuelPrices([FromQuery] DateTime raporTarihi)
        {
            Task<Core.Utilities.Results.IResult> collectApiTask = _fuelImportService.ImportAndSaveFuelPricesAsync();

            Task<Core.Utilities.Results.IResult> epdkTask = _epdkBultenImportService.ImportAndSaveTurkeyFuelPricesAsync(raporTarihi);


            await Task.WhenAll(collectApiTask, epdkTask);

            var collectApiResult = await collectApiTask;
            var epdkResult = await epdkTask;

            // Her iki işlemden biri bile başarısızsa hata detayı dönüyoruz
            if (!collectApiResult.Success || !epdkResult.Success)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Bazı servislerden veri aktarılırken hata oluştu.",
                    CollectApiDetails = collectApiResult,
                    EpdkDetails = epdkResult
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Tüm yakıt fiyatı verileri (Türkiye + Avrupa) başarıyla çekildi ve veritabanına kaydedildi.",
                CollectApiDetails = collectApiResult,
                EpdkDetails = epdkResult
            });
        }
    }
}

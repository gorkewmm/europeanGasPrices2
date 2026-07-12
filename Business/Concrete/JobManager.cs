using Business.Abstract;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Service.Abstract;

namespace Business.Concrete
{
    public class JobManager : IJobService
    {
        private readonly IFuelImportService _fuelImportService;
        private readonly ILogger<JobManager> _logger; // Logger tanımı
        public JobManager(IFuelImportService fuelImportService, ILogger<JobManager> logger)
        {
            _fuelImportService = fuelImportService;
            _logger = logger;
        }
        public void Jobs()
        {
            try
            {
                _logger.LogInformation("Aktif recurring job'lar Hangfire'a kaydediliyor...");

                //RecurringJob.AddOrUpdate<IFuelImportService>(
                //    "OtomatikAkaryakitVeriCekmeGörevi5",
                //    x => x.ImportAndSaveFuelPricesAsync(),
                //    "55 12 * * *",
                //    new RecurringJobOptions
                //    {
                //        TimeZone = TimeZoneInfo.Local
                //    });

                RecurringJob.AddOrUpdate<IFuelImportService>(
                    "OtomatikAkaryakitVeriCekmeGörevi6",
                    x => x.ImportAndSaveFuelPricesAsync(),
                    "54 12 * * *",
                    new RecurringJobOptions
                    {
                        TimeZone = TimeZoneInfo.Local
                    });

                _logger.LogInformation("Aktif recurring job'lar başarıyla kaydedildi.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Aktif job'lar kaydedilirken kritik bir hata oluştu!");
            }
        }

        /// <summary>
        /// Eski/kullanılmayan job'ları Hangfire'dan kaldırır.
        /// </summary>
        public void RemoveJobs()
        {
            try
            {
                RecurringJob.RemoveIfExists("OtomatikAkaryakitVeriCekmeGörevi");
                RecurringJob.RemoveIfExists("OtomatikAkaryakitVeriCekmeGörevi2");
                RecurringJob.RemoveIfExists("OtomatikAkaryakitVeriCekmeGörevi3");
                RecurringJob.RemoveIfExists("OtomatikAkaryakitVeriCekmeGörevi4");
                RecurringJob.RemoveIfExists("OtomatikAkaryakitVeriCekmeGörevi5");

                _logger.LogInformation("Eski recurring job'lar temizlendi.");
            }
            catch (Exception ex)
            {
                // Görseldeki gibi: İlk çalıştırmada joblar veritabanında hiç olmayabileceği için Warning (Uyarı) seviyesinde logluyoruz
                _logger.LogWarning(ex, "Eski job'lar temizlenirken hata oluştu (ilk çalıştırma olabilir).");
            }

        }
    }
}

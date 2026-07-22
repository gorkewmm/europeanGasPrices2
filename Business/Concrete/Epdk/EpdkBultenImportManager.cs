using Business.Abstract;
using Business.Abstract.Epdk;
using Core.Utilities.Results;
using Entities.DTOs;
using Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Business.Concrete.Epdk
{
    public class EpdkBultenImportManager : IEpdkBultenImportService
    {
        string baseUrl = "https://apigateway.epdk.gov.tr/petrolBayiSatisFiyatBulten/";

        private readonly HttpClient _httpClient;
        private readonly IFuelPriceService _fuelPriceService;
        public EpdkBultenImportManager(HttpClient httpClient, IFuelPriceService fuelPriceService)
        {
            _httpClient = httpClient;
            _fuelPriceService = fuelPriceService;
        }

        public async Task<EpdkBultenResponseDto> GetBultenFromApiAsync(DateTime? raporTarihi)
        {
            var targetDate = raporTarihi ?? DateTime.Now;

            var requstBody = new
            {
                raporTarihi = targetDate.ToString("yyyy-MM-dd")
            };

            var jsonContent = JsonSerializer.Serialize(requstBody);


            var request = new HttpRequestMessage(HttpMethod.Get, baseUrl);

            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"EPDK API hata döndürdü! Durum Kodu: {response.StatusCode} - Nedeni: {response.ReasonPhrase}");
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            var epdkBultenResponseDto = JsonSerializer.Deserialize<EpdkBultenResponseDto>(jsonString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return epdkBultenResponseDto;
        }


        public async Task<IResult> ImportAndSaveTurkeyFuelPricesAsync(DateTime raporTarihi)
        {
            var result = await GetBultenFromApiAsync(raporTarihi);
            if (result == null || result.Data == null || result.Data.Count == 0)
            {
                return new ErrorResult("EPDK API'den veri alınamadı veya veri boş.");

            }

            var fuelPrice = EpdkBultenMapper.ToTurkeyFuelPriceEntity(result);

            if (fuelPrice == null)
            {
                return new ErrorResult("EPDK verisi işlenirken bir hata oluştu.");
            }

            // 3. İç servis üzerinden Entity'i veritabanına kaydet
            var saveResult = _fuelPriceService.Add(fuelPrice);

            return saveResult;
        }
    }
}
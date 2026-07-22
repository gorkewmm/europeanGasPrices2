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

        public async Task<EpdkBultenResponseDto> GetBultenFromApiAsync(DateTime raporTarihi)
        {
            DateTime targetDate = (raporTarihi == default) ? DateTime.Now : raporTarihi;

            // 1. EPDK'nın istediği format: dd.MM.yyyy (Örn: 02.02.2026)
            string formattedDate = targetDate.ToString("dd.MM.yyyy");

            // 2. GET isteğinde gövde (body) kabul eden EPDK için json hazırlığı
            var requestBody = new
            {
                raporTarihi = formattedDate
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);


            var request = new HttpRequestMessage(HttpMethod.Get, baseUrl)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
                // EPDK gibi eski/özel gateway'lerin GET+Body kabul etmesi için HTTP 1.1 zorunlu kılınır
                Version = System.Net.HttpVersion.Version11
            };



            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"EPDK API Hata Dönüştü! Kod: {response.StatusCode} - Detay: {errorDetails}");
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

            var fuelPrice = EpdkBultenMapper.ToTurkeyFuelPriceEntity(result,raporTarihi);

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
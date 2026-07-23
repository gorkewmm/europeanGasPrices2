using Business.Abstract.Epdk;
using Core.Utilities.Results;
using Entities.DTOs.Epdk;
using Entities.Mappers.Epdk;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Business.Concrete.Epdk
{
    public class EpdkImportManager : IEpdkImportService
    {
        private readonly HttpClient _httpClient;
        private readonly IEpdkFuelPriceService _epdkFuelPriceService;
        public EpdkImportManager(HttpClient httpClient, IEpdkFuelPriceService epdkFuelPriceService)
        {
            _httpClient = httpClient;
            _epdkFuelPriceService = epdkFuelPriceService;
        }
        public async Task<EpdkBultenResponseDto> GetEpdkFuelPricesFromApi(DateTime raporTarihi)
        {
            var baseUrl = "https://apigateway.epdk.gov.tr/petrolBayiSatisFiyatBulten/";

            var targetDate = (raporTarihi == default) ? DateTime.Now : raporTarihi;

            var requestBody = new
            {
                raporTarihi = targetDate.ToString("dd.MM.yyyy")
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);

            var req = new HttpRequestMessage(HttpMethod.Get, baseUrl)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
                Version = System.Net.HttpVersion.Version11
            };


            //dönen yanıt
            var response = await _httpClient.SendAsync(req);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"EPDK API Hata Dönüştü! Kod: {response.StatusCode} - Detay: {errorDetails}");
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            var epdkBultenResponseDto =  JsonSerializer.Deserialize<EpdkBultenResponseDto>(jsonString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return epdkBultenResponseDto;

        }

        public async Task<IResult> ImportAndSaveEpdkFuelPricesAsync(DateTime raporTarihi)
        {
            var epdkBultenResponseDto = await GetEpdkFuelPricesFromApi(raporTarihi);

            if(epdkBultenResponseDto == null || epdkBultenResponseDto.Data == null || epdkBultenResponseDto.Data.Count == 0)
            {
                return new ErrorResult("EPDK API'den veri alınamadı veya veri boş.");
            }

            var listEpdkFuelPrice = EpdkFuelPriceMapper.ToEntity(epdkBultenResponseDto);

            var saveResult = _epdkFuelPriceService.AddPriceHistoryIfChanged(listEpdkFuelPrice);

            return saveResult;
            
        }
    }
}

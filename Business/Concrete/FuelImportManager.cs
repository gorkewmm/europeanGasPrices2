using Business.Abstract;
using Core.Utilities.Results;
using Entities.DTOs;
using System.Text.Json;
using WebAPI.Service.Abstract;

namespace Business.Concrete
{
    public class FuelImportManager : IFuelImportService
    {
        private readonly HttpClient _httpClient;
        private readonly IFuelPriceService _fuelPriceService;
        public FuelImportManager(HttpClient httpClient, IFuelPriceService fuelPriceService)
        {
            _httpClient = httpClient;
            _fuelPriceService = fuelPriceService;
        }
        public async Task<List<FuelPriceApiDto>> GetFuelPricesFromApi()
        {
            var url = "https://api.collectapi.com/gasPrice/europeanCountries";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Add("authorization", "apikey 5UE1tDNuPMBzqZvrjFS1Wr:4Vdi951lcl8zmDXFNb8Kun");
            

            
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API hata döndürdü! Durum Kodu: {response.StatusCode} - Nedeni: {response.ReasonPhrase}");
            }

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<FuelPriceResponseDto>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result.Result;
        }

        public async Task<IResult> ImportAndSaveFuelPricesAsync()
        {
            // 1. Dış API'den verileri DTO olarak çek
            List<FuelPriceApiDto> fuelPriceApiDtos = await GetFuelPricesFromApi();

            if (fuelPriceApiDtos == null || fuelPriceApiDtos.Count == 0)
            {
                return new ErrorResult("API'den veri alınamadı.");
            }

            // 2. Kendi içindeki diğer Business servisini çağırarak kaydet
            var result = _fuelPriceService.AddFromApi(fuelPriceApiDtos);

            return result;
        }
    }
}

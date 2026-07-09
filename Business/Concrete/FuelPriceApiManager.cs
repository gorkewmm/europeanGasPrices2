using Business.Abstract;
using Core.Utilities.Results;
using Entities.DTOs;
using System.Text.Json;
using WebAPI.Service.Abstract;

namespace WebAPI.Service.Concrete
{
    public class FuelPriceApiManager : IFuelPriceApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IFuelPriceService _fuelPriceService;
        public FuelPriceApiManager(HttpClient httpClient, IFuelPriceService fuelPriceService)
        {
            _httpClient = httpClient;
            _fuelPriceService = fuelPriceService;
        }
        public async Task<List<FuelPriceDto>> GetFuelPricesFromApi()
        {
            var url = "https://api.collectapi.com/gasPrice/europeanCountries";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Add("authorization", "apikey 4Hk3rgvqx1DovXqt3sk9Q7:1YllFKd1uzhe8RkNSCv3rj");
            

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("API bağlantı hatası");
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
            List<FuelPriceDto> fuelPriceDtos = await GetFuelPricesFromApi();

            if (fuelPriceDtos == null || fuelPriceDtos.Count == 0)
            {
                return new ErrorResult("API'den veri alınamadı.");
            }

            // 2. Kendi içindeki diğer Business servisini çağırarak kaydet
            var result = _fuelPriceService.AddFromApi(fuelPriceDtos);

            return result;
        }
    }
}

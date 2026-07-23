using System.Text.Json.Serialization;

namespace Entities.DTOs.Epdk
{
    public class EpdkBultenItemDto
    {
        [JsonPropertyName("Fiyat")]
        public decimal Fiyat { get; set; }


        [JsonPropertyName("Yakıt")]
        public string Yakit { get; set; }


        [JsonPropertyName("Tarih")]
        public string Tarih { get; set; }


        [JsonPropertyName("Ölçü Birimi")]
        public string OlcuBirimi { get; set; }


    }
}

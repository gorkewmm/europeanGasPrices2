using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Entities.DTOs
{
    public class EpdkBultenResponseDto
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }


        [JsonPropertyName("statusDescription")]
        public string StatusDescription { get; set; }


        [JsonPropertyName("message")]
        public string Message { get; set; }


        [JsonPropertyName("columnNames")]
        public List<string> ColumnNames { get; set; }


        [JsonPropertyName("numRows")]
        public int NumRows { get; set; }


        [JsonPropertyName("data")]
        public List<EpdkBultenItemDto> Data { get; set; }


        [JsonPropertyName("elapsedTime")]
        public int ElapsedTime { get; set; }


        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; }
    }


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

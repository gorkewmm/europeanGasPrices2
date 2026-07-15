using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class FuelPriceResponseDto : IDto
    {
        public bool Success { get; set; }

        public List<FuelPriceDto> Result { get; set; } = new();
    }
}
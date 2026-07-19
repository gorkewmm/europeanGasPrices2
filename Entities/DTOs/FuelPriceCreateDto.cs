using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class FuelPriceCreateDto : IDto
    {
        public string Country { get; set; } = null!;
        public string Currency { get; set; } = null!;

        public decimal Gasoline { get; set; }
        public decimal Diesel { get; set; }
        public decimal Lpg { get; set; }
    }
}

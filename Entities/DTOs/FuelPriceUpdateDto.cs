using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class FuelPriceUpdateDto : IDto
    {
        public int Id { get; set; }

        public string Country { get; set; } = null!;
        public string Currency { get; set; } = null!;

        public decimal Gasoline { get; set; }
        public decimal Diesel { get; set; }
        public decimal Lpg { get; set; }
    }
}

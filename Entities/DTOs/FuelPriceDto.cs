using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class FuelPriceDto : IDto
    {
        public string Country { get; set; } = null!;

        public string Currency { get; set; } = null!;

        public string Gasoline { get; set; } = null!;

        public string Diesel { get; set; } = null!;

        public string Lpg { get; set; } = null!;
    }
}
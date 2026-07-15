using Core.Entities;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class FuelPrice : BaseEntity, IEntity
    {
        public string Country { get; set; } = null!;

        public string Currency { get; set; } = null!;

        public decimal Gasoline { get; set; }

        public decimal Diesel { get; set; }

        public decimal Lpg { get; set; }

        public DateTime PriceDate { get; set; }
    }
}
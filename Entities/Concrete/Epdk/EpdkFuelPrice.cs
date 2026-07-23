using Core.Entities;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete.Epdk
{
    public class EpdkFuelPrice : BaseEntity, IEntity
    {
        public decimal Fiyat { get; set; }
        public string Yakit { get; set; }
        public string Tarih { get; set; }
        public string OlcuBirimi { get; set; }
        public DateTime PriceDate { get; set; }
    }
}

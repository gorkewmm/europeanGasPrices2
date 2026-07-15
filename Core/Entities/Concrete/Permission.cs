using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Concrete
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; }        // Örn: "fuelprice.add"
        public string Description { get; set; } // Örn: "Kullanıcının sisteme yeni akaryakıt fiyatı eklemesini sağlar."
    }
}
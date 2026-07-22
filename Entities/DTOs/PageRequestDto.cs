using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class PageRequestDto 
    {
        public int PageIndex { get; set; } = 0; // 0dan başlar ve 0 = 1. sayfa
        public int PageSize { get; set; } = 10; // Bir sayfadaki kayıt sayısı
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; } = new();
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPrevious => PageIndex > 0;
        public bool HasNext => PageIndex + 1 < TotalPages;

    }
}
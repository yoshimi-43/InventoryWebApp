using System.Collections.Generic;

namespace InventoryWebApp.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public int TotalPages => (int)System.Math.Ceiling(TotalCount / (double)PageSize);
    }
}

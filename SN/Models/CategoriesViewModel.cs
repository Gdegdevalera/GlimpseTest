using SN.Data;
using System.Collections.Generic;

namespace SN.Models
{
    public class CategoriesViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<CategoryByHour> CategoriesByHour { get; set; }
    }
}

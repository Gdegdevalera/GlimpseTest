using Microsoft.EntityFrameworkCore;

namespace SN.Data
{
    [Index("Name", IsUnique = true)]
    public class Category
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long Total { get; set; }
    }
}

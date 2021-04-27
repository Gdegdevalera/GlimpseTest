using Microsoft.EntityFrameworkCore;
using System;

namespace SN.Data
{
    [Index(new[] { "Name", "Hour" }, IsUnique = true)]
    public class CategoryByHour
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long Total { get; set; }

        public DateTimeOffset Hour { get; set; }
    }
}

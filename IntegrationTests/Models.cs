using System;
using System.Collections.Generic;

namespace IntegrationTests
{
    public class CategoriesViewModel
    {
        public List<Category> Categories { get; set; }
    }

    public class Category
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long Total { get; set; }

        public DateTimeOffset Hour { get; set; }
    }
}

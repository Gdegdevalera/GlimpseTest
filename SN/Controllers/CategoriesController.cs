using Microsoft.AspNetCore.Mvc;
using SN.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SN.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return _db.Categories;
        }

        [HttpGet("by-hours/{name?}")]
        public IEnumerable<CategoryByHour> GetPerHour(string name)
        {
            var now = DateTimeOffset.UtcNow.AddDays(-1);

            var result = _db.CategoriesByHour.Where(x => x.Hour >= now);

            if (name != null)
            {
                result = result.Where(x => x.Name == name);
            }

            return result;
        }
    }
}

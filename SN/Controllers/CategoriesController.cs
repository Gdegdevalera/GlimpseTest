using Microsoft.AspNetCore.Mvc;
using SN.Data;
using SN.Models;
using System;
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
        public CategoriesViewModel Get()
        {
            var now = DateTimeOffset.UtcNow.AddDays(-1);

            return new CategoriesViewModel
            {
                Categories = _db.Categories,
                CategoriesByHour = _db.CategoriesByHour.Where(x => x.Hour >= now)
            };
        }
    }
}

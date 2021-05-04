using Microsoft.AspNetCore.Mvc;
using SN.Models;
using SN.Services;

namespace SN.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationService _service;

        public CategoriesController(ApplicationService service)
        {
            _service = service;
        }

        [HttpGet]
        public CategoriesViewModel Get() => _service.GetCategories();
    }
}

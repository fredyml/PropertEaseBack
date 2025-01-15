using Microsoft.AspNetCore.Mvc;
using PropertEase.Application.Interfaces;

namespace PropertEase.Controllers
{
    [ApiController]
    [Route("api/properties")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertiesController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }
        

        [HttpGet]
        public async Task<IActionResult> GetProperties( string? name = null,string? address = null,decimal? minPrice = null, decimal? maxPrice = null)
        {
            var properties = await _propertyService.GetFilteredPropertiesAsync(name, address, minPrice, maxPrice);
            return Ok(properties);
        }

    }
}

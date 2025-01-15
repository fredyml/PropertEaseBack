using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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

        [HttpGet("{idProperty}")]
        public async Task<IActionResult> GetPropertyById(string idProperty)
        {
            if (!ObjectId.TryParse(idProperty, out var objectId))
                return BadRequest(new { Message = "Invalid ID format." });

            var property = await _propertyService.GetPropertyByIdAsync(objectId);
            if (property == null)
                return NotFound(new { Message = "Property not found." });

            return Ok(property);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProperties()
        {
            var properties = await _propertyService.GetAllPropertiesAsync();
            return Ok(properties);
        }
    }
}

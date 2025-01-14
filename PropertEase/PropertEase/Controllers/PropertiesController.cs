using Microsoft.AspNetCore.Mvc;
using PropertEase.Application.Interfaces;
using Serilog;

namespace PropertEase.Controllers
{
    // Controlador
    [ApiController]
    [Route("api/properties")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _service;

        public PropertiesController(IPropertyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string name, [FromQuery] string address, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            try
            {
                var properties = await _service.GetPropertiesAsync(name, address, minPrice, maxPrice);
                return Ok(properties);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error en el controlador");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }
    }
}

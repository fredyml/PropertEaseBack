using Microsoft.AspNetCore.Mvc;
using PropertEase.Application.Dtos;
using PropertEase.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>
        /// Obtiene una lista de propiedades filtradas por los parámetros proporcionados.
        /// </summary>
        /// <remarks>
        /// Este endpoint devuelve una lista de propiedades basada en filtros como nombre, dirección, y precio mínimo y máximo.
        /// Si no se proporciona un filtro, se devolverán todas las propiedades disponibles.
        /// </remarks>
        /// <param name="name">Nombre de la propiedad (opcional)</param>
        /// <param name="address">Dirección de la propiedad (opcional)</param>
        /// <param name="minPrice">Precio mínimo de la propiedad (opcional)</param>
        /// <param name="maxPrice">Precio máximo de la propiedad (opcional)</param>
        /// <param name="page">El número de página para la paginación (opcional, por defecto es 1).</param>
        /// <param name="pageSize">La cantidad de propiedades por página (opcional, por defecto es 10).</param>
        /// <returns>Lista de propiedades que cumplen con los filtros especificados</returns>
        /// <response code="200">Devuelve la lista de propiedades</response>
        /// <response code="400">Si los parámetros de entrada no son válidos</response>
        /// <response code="500">Si ocurre un error en el servidor</response>
        [HttpGet]
        [SwaggerOperation(summary:"Obtiene una lista de propiedades filtradas")]
        [SwaggerResponse(200, "Lista de propiedades obtenida correctamente.", typeof(IEnumerable<PropertyDto>))]
        [SwaggerResponse(400, "Parámetros de entrada no válidos.")]
        [SwaggerResponse(500, "Error interno del servidor.")]
        public async Task<IActionResult> GetProperties(
           [FromQuery] string? name = null,
           [FromQuery] string? address = null,
           [FromQuery] decimal? minPrice = null,
           [FromQuery] decimal? maxPrice = null,
           [FromQuery] int? page = null,       
           [FromQuery] int? pageSize = null)  
        {
            if (!page.HasValue) page = 1;     
            if (!pageSize.HasValue) pageSize = 10;

            var properties = await _propertyService.GetFilteredPropertiesAsync(name, address, minPrice, maxPrice, page.Value, pageSize.Value);
            return Ok(properties);
        }

    }
}

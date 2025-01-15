using AutoMapper;
using PropertEase.Application.Dtos;
using PropertEase.Application.Interfaces;

namespace PropertEase.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Crea una nueva instancia de <see cref="PropertyService"/>.
        /// </summary>
        /// <param name="propertyRepository">Repositorio de propiedades utilizado para obtener las propiedades filtradas.</param>
        /// <param name="mapper">El mapeador utilizado para convertir las entidades a DTOs.</param>
        public PropertyService(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene una lista de propiedades filtradas según los parámetros proporcionados.
        /// </summary>
        /// <param name="name">El nombre de la propiedad para filtrar (opcional).</param>
        /// <param name="address">La dirección de la propiedad para filtrar (opcional).</param>
        /// <param name="minPrice">El precio mínimo para filtrar las propiedades (opcional).</param>
        /// <param name="maxPrice">El precio máximo para filtrar las propiedades (opcional).</param>
        /// <returns>Una lista de objetos <see cref="PropertyDto"/> que cumplen con los filtros especificados.</returns>
        /// <remarks>
        /// Este método consulta el repositorio de propiedades y aplica los filtros indicados
        /// para obtener solo las propiedades que coinciden con los criterios.
        /// Si no se proporcionan parámetros de filtro, se devolverán todas las propiedades disponibles.
        /// </remarks>
        /// <example>
        /// Ejemplo de uso:
        /// var properties = await propertyService.GetFilteredPropertiesAsync("Casa", null, 100000, 500000);
        /// </example>
        public async Task<IEnumerable<PropertyDto>> GetFilteredPropertiesAsync(
            string? name,
            string? address,
            decimal? minPrice,
            decimal? maxPrice)
        {
            var properties = await _propertyRepository.GetFilteredPropertiesAsync(name, address, minPrice, maxPrice);
            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }
    }
}

using PropertEase.Domain.Entities;

namespace PropertEase.Application.Interfaces
{
    public interface IPropertyRepository
    {
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
        Task<IEnumerable<Property>> GetFilteredPropertiesAsync(string? name,
            string? address,
            decimal? minPrice,
            decimal? maxPrice,
            int page,
            int pageSize);
    }
}

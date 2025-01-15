using MongoDB.Bson;
using MongoDB.Driver;
using PropertEase.Application.Interfaces;
using PropertEase.Domain.Entities;

namespace PropertEase.Infrastructure.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly IMongoCollection<Property> _properties;
        private readonly IMongoCollection<Owner> _owners;
        private readonly IMongoCollection<PropertyImage> _images;
        private readonly IMongoCollection<PropertyTrace> _traces;

        /// <summary>
        /// Crea una nueva instancia de <see cref="PropertyRepository"/>.
        /// </summary>
        /// <param name="client">Cliente de MongoDB que se utilizará para acceder a la base de datos.</param>
        public PropertyRepository(IMongoClient client)
        {
            var database = client.GetDatabase("RealEstateDb");
            _properties = database.GetCollection<Property>("Properties");
            _owners = database.GetCollection<Owner>("Owners");
            _images = database.GetCollection<PropertyImage>("PropertyImages");
            _traces = database.GetCollection<PropertyTrace>("PropertyTraces");
        }

        /// <summary>
        /// Obtiene una lista de propiedades filtradas según los parámetros proporcionados.
        /// </summary>
        /// <param name="name">El nombre de la propiedad para filtrar (opcional).</param>
        /// <param name="address">La dirección de la propiedad para filtrar (opcional).</param>
        /// <param name="minPrice">El precio mínimo para filtrar las propiedades (opcional).</param>
        /// <param name="maxPrice">El precio máximo para filtrar las propiedades (opcional).</param>
        /// <returns>Una lista de propiedades que cumplen con los filtros especificados.</returns>
        /// <remarks>
        /// Si no se proporcionan filtros, se devolverán todas las propiedades disponibles.
        /// Los filtros de precio y texto se aplican de forma acumulativa, es decir, todos deben coincidir.
        /// </remarks>
        /// <example>
        /// Ejemplo de uso:
        /// var properties = await propertyRepository.GetFilteredPropertiesAsync("Casa", "Madrid", 100000, 500000);
        /// </example>
        public async Task<IEnumerable<Property>> GetFilteredPropertiesAsync(
            string? name,
            string? address,
            decimal? minPrice,
            decimal? maxPrice)
        {
            var filterBuilder = Builders<Property>.Filter.Empty;

            if (!string.IsNullOrEmpty(name))
            {
                filterBuilder &= Builders<Property>.Filter.Regex(p => p.Name, new BsonRegularExpression(name, "i"));
            }

            if (!string.IsNullOrEmpty(address))
            {
                filterBuilder &= Builders<Property>.Filter.Regex(p => p.Address, new BsonRegularExpression(address, "i"));
            }

            if (minPrice.HasValue)
            {
                filterBuilder &= Builders<Property>.Filter.Gte(p => p.Price, minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                filterBuilder &= Builders<Property>.Filter.Lte(p => p.Price, maxPrice.Value);
            }

            var properties = await _properties.Find(filterBuilder).ToListAsync();

            foreach (var property in properties)
            {
                property.Images = await GetImagesByIdsAsync(property.ImageIds);
                property.Traces = await GetTracesByIdsAsync(property.TraceIds);
                property.Owner = await GetOwnerByIdAsync(property.IdOwner);
            }

            return properties;
        }

        /// <summary>
        /// Obtiene una lista de imágenes relacionadas con las propiedades mediante sus IDs.
        /// </summary>
        /// <param name="imageIds">Una lista de identificadores de imágenes a buscar.</param>
        /// <returns>Una lista de objetos <see cref="PropertyImage"/> correspondientes a los IDs proporcionados.</returns>
        /// <remarks>
        /// Este método consulta la colección de imágenes de la base de datos para obtener las imágenes asociadas.
        /// </remarks>
        private async Task<List<PropertyImage>> GetImagesByIdsAsync(IEnumerable<ObjectId> imageIds)
        {
            return await _images.Find(img => imageIds.Contains(img.Id)).ToListAsync();
        }

        /// <summary>
        /// Obtiene una lista de trazas relacionadas con las propiedades mediante sus IDs.
        /// </summary>
        /// <param name="traceIds">Una lista de identificadores de trazas a buscar.</param>
        /// <returns>Una lista de objetos <see cref="PropertyTrace"/> correspondientes a los IDs proporcionados.</returns>
        /// <remarks>
        /// Este método consulta la colección de trazas en la base de datos para obtener las trazas asociadas.
        /// </remarks>
        private async Task<List<PropertyTrace>> GetTracesByIdsAsync(IEnumerable<ObjectId> traceIds)
        {
            return await _traces.Find(trace => traceIds.Contains(trace.Id)).ToListAsync();
        }

        /// <summary>
        /// Obtiene el propietario de una propiedad mediante su ID.
        /// </summary>
        /// <param name="ownerId">El identificador del propietario a buscar.</param>
        /// <returns>El objeto <see cref="Owner"/> correspondiente al ID proporcionado.</returns>
        /// <remarks>
        /// Este método consulta la colección de propietarios en la base de datos para obtener el propietario asociado.
        /// </remarks>
        private async Task<Owner> GetOwnerByIdAsync(ObjectId ownerId)
        {
            return await _owners.Find(o => o.Id == ownerId).FirstOrDefaultAsync();
        }
    }
}

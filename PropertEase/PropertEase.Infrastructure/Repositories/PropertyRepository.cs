using MongoDB.Bson;
using MongoDB.Driver;
using PropertEase.Application.Interfaces;
using PropertEase.Domain.Entities;
using PropertEase.Domain.Exceptions;

namespace PropertEase.Infrastructure.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly IMongoCollection<Property> _properties;
        private readonly IMongoCollection<Owner> _owners;
        private readonly IMongoCollection<PropertyImage> _images;
        private readonly IMongoCollection<PropertyTrace> _traces;

        public PropertyRepository(IMongoClient client)
        {
            var database = client.GetDatabase("RealEstateDb");
            _properties = database.GetCollection<Property>("Properties");
            _owners = database.GetCollection<Owner>("Owners");
            _images = database.GetCollection<PropertyImage>("PropertyImages");
            _traces = database.GetCollection<PropertyTrace>("PropertyTraces");
        }

        /// <summary>
        /// Obtiene una lista de propiedades filtradas y paginadas según los parámetros proporcionados.
        /// </summary>
        /// <param name="name">El nombre de la propiedad para filtrar (opcional).</param>
        /// <param name="address">La dirección de la propiedad para filtrar (opcional).</param>
        /// <param name="minPrice">El precio mínimo para filtrar las propiedades (opcional).</param>
        /// <param name="maxPrice">El precio máximo para filtrar las propiedades (opcional).</param>
        /// <param name="page">El número de página para la paginación (opcional, por defecto es 1).</param>
        /// <param name="pageSize">La cantidad de propiedades por página (opcional, por defecto es 10).</param>
        /// <returns>Una lista de propiedades que cumplen con los filtros y están paginadas según el número de página y el tamaño de página.</returns>
        public async Task<IEnumerable<Property>> GetFilteredPropertiesAsync(
            string? name,
            string? address,
            decimal? minPrice,
            decimal? maxPrice,
            int page,
            int pageSize)
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

            var skip = (page - 1) * pageSize; 
            var properties = await _properties.Find(filterBuilder)
                                              .Skip(skip)       
                                              .Limit(pageSize)   
                                              .ToListAsync();

            if (properties == null || !properties.Any())
            {
                throw new PropertyNotFoundException("No properties were found matching the given filters.");
            }


            foreach (var property in properties)
            {
                property.Images = await GetImagesByIdsAsync(property.ImageIds);
                property.Traces = await GetTracesByIdsAsync(property.TraceIds);
                property.Owner = await GetOwnerByIdAsync(property.IdOwner);
            }

            return properties;
        }

        private async Task<List<PropertyImage>> GetImagesByIdsAsync(IEnumerable<ObjectId> imageIds)
        {
            return await _images.Find(img => imageIds.Contains(img.Id)).ToListAsync();
        }

        private async Task<List<PropertyTrace>> GetTracesByIdsAsync(IEnumerable<ObjectId> traceIds)
        {
            return await _traces.Find(trace => traceIds.Contains(trace.Id)).ToListAsync();
        }

        private async Task<Owner> GetOwnerByIdAsync(ObjectId ownerId)
        {
            return await _owners.Find(o => o.Id == ownerId).FirstOrDefaultAsync();
        }
    }
}

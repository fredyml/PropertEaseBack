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
        public async Task<IEnumerable<Property>> GetFilteredPropertiesAsync(
            string? name,
            string? address,
            decimal? minPrice,
            decimal? maxPrice,
            int page = 1,
            int pageSize = 10)
        {
            var filterBuilder = BuildPropertyFilter(name, address, minPrice, maxPrice);
            var properties = await GetPaginatedProperties(filterBuilder, page, pageSize);

            if (properties == null || !properties.Any())
            {
                throw new PropertyNotFoundException("No properties were found matching the given filters.");
            }

            await EnrichPropertiesAsync(properties);
            return properties;
        }

        /// <summary>
        /// Construye el filtro para la búsqueda de propiedades.
        /// </summary>
        private FilterDefinition<Property> BuildPropertyFilter(
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

            return filterBuilder;
        }

        /// <summary>
        /// Obtiene propiedades paginadas según el filtro proporcionado.
        /// </summary>
        private async Task<List<Property>> GetPaginatedProperties(
            FilterDefinition<Property> filter,
            int page,
            int pageSize)
        {
            var skip = (page - 1) * pageSize;
            return await _properties.Find(filter)
                                    .Skip(skip)
                                    .Limit(pageSize)
                                    .ToListAsync();
        }

        /// <summary>
        /// Enriquecemos las propiedades con imágenes, trazas y propietario.
        /// </summary>
        private async Task EnrichPropertiesAsync(IEnumerable<Property> properties)
        {
            foreach (var property in properties)
            {
                if (property.Images.Count == 0)
                    property.Images.Add(await GetImagesByIdsAsync(property.ImageIds));

                property.Traces = await GetTracesByIdsAsync(property.TraceIds);
                property.Owner = await GetOwnerByIdAsync(property.IdOwner);
            }
        }

        /// <summary>
        /// Obtiene las imágenes por los identificadores proporcionados.
        /// </summary>
        private async Task<PropertyImage> GetImagesByIdsAsync(IEnumerable<ObjectId> imageIds)
        {
            return await _images.Find(img => imageIds.Contains(img.Id)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Obtiene las trazas por los identificadores proporcionados.
        /// </summary>
        private async Task<List<PropertyTrace>> GetTracesByIdsAsync(IEnumerable<ObjectId> traceIds)
        {
            return await _traces.Find(trace => traceIds.Contains(trace.Id)).ToListAsync();
        }

        /// <summary>
        /// Obtiene el propietario por el identificador proporcionado.
        /// </summary>
        private async Task<Owner> GetOwnerByIdAsync(ObjectId ownerId)
        {
            return await _owners.Find(o => o.Id == ownerId).FirstOrDefaultAsync();
        }
    }
}

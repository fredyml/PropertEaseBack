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

        public PropertyRepository(IMongoClient client)
        {
            var database = client.GetDatabase("RealEstateDb");
            _properties = database.GetCollection<Property>("Properties");
            _owners = database.GetCollection<Owner>("Owners");
            _images = database.GetCollection<PropertyImage>("PropertyImages");
            _traces = database.GetCollection<PropertyTrace>("PropertyTraces");
        }

        public async Task<IEnumerable<Property>> GetFilteredPropertiesAsync(string? name, string? address, decimal? minPrice,decimal? maxPrice)
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

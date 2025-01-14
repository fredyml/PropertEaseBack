using MongoDB.Bson;
using MongoDB.Driver;
using PropertEase.Application.Interfaces;
using PropertEase.Domain.Entities;

namespace PropertEase.Infrastructure.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly IMongoCollection<Property> _properties;

        public PropertyRepository(IMongoClient client)
        {
            var database = client.GetDatabase("RealEstateDb");
            _properties = database.GetCollection<Property>("Properties");
        }

        public async Task<IEnumerable<Property>> GetPropertiesAsync(string name, string address, decimal? minPrice, decimal? maxPrice)
        {
            var filterBuilder = Builders<Property>.Filter;
            var filters = new List<FilterDefinition<Property>>();

            if (!string.IsNullOrEmpty(name))
                filters.Add(filterBuilder.Regex("Name", new BsonRegularExpression(name, "i")));

            if (!string.IsNullOrEmpty(address))
                filters.Add(filterBuilder.Regex("Address", new BsonRegularExpression(address, "i")));

            if (minPrice.HasValue)
                filters.Add(filterBuilder.Gte("Price", minPrice.Value));

            if (maxPrice.HasValue)
                filters.Add(filterBuilder.Lte("Price", maxPrice.Value));

            var filter = filters.Count > 0 ? filterBuilder.And(filters) : FilterDefinition<Property>.Empty;
            return await _properties.Find(filter).ToListAsync();
        }
    }
}

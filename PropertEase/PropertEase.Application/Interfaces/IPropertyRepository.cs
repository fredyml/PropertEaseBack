using PropertEase.Domain.Entities;
using MongoDB.Bson;

namespace PropertEase.Application.Interfaces
{
    public interface IPropertyRepository
    {
        Task<Property> GetPropertyByIdAsync(ObjectId id);  
        Task<IEnumerable<Property>> GetAllPropertiesAsync();
    }
}

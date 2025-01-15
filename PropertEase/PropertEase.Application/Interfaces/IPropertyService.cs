using MongoDB.Bson;
using PropertEase.Application.Dtos;

namespace PropertEase.Application.Interfaces
{
    public interface IPropertyService
    {
        Task<PropertyDto> GetPropertyByIdAsync(ObjectId idProperty);
        Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync();
    }
}

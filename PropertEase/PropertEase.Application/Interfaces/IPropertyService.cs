using PropertEase.Domain.Entities;

namespace PropertEase.Application.Interfaces
{
    public interface IPropertyService
    {
        Task<IEnumerable<Property>> GetPropertiesAsync(string name, string address, decimal? minPrice, decimal? maxPrice);
    }
}

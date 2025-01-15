using PropertEase.Domain.Entities;

namespace PropertEase.Application.Interfaces
{
    public interface IPropertyRepository
    {
        Task<IEnumerable<Property>> GetFilteredPropertiesAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice);
    }
}

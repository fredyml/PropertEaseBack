using PropertEase.Application.Dtos;

namespace PropertEase.Application.Interfaces
{
    public interface IPropertyService
    {
        Task<IEnumerable<PropertyDto>> GetFilteredPropertiesAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice, int page, int pageSize);
    }
}

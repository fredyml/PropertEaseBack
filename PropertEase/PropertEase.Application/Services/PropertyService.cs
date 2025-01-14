using PropertEase.Application.Interfaces;
using PropertEase.Domain.Entities;
using Serilog;

namespace PropertEase.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _repository;

        public PropertyService(IPropertyRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Property>> GetPropertiesAsync(string name, string address, decimal? minPrice, decimal? maxPrice)
        {
            try
            {
                return await _repository.GetPropertiesAsync(name, address, minPrice, maxPrice);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener propiedades");
                throw;
            }
        }
    }
}

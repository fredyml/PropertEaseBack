using AutoMapper;
using PropertEase.Application.Dtos;
using PropertEase.Application.Interfaces;

namespace PropertEase.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public PropertyService(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }
       

        public async Task<IEnumerable<PropertyDto>> GetFilteredPropertiesAsync(string? name, string? address,decimal? minPrice,decimal? maxPrice)
        {
            var properties = await _propertyRepository.GetFilteredPropertiesAsync(name, address, minPrice, maxPrice);
            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }

    }
}

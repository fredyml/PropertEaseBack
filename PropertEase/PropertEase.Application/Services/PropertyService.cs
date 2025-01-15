using AutoMapper;
using MongoDB.Bson;
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

        public async Task<PropertyDto?> GetPropertyByIdAsync(ObjectId id)
        {
            var property = await _propertyRepository.GetPropertyByIdAsync(id);
            return property == null ? null : _mapper.Map<PropertyDto>(property);
        }

        public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync()
        {
            var properties = await _propertyRepository.GetAllPropertiesAsync();
            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }
    }
}

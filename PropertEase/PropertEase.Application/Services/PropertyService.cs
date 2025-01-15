using MongoDB.Bson;
using PropertEase.Application.Dtos;
using PropertEase.Application.Interfaces;
using PropertEase.Domain.Entities;

namespace PropertEase.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;

        public PropertyService(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<PropertyDto?> GetPropertyByIdAsync(ObjectId id)
        {
            var property = await _propertyRepository.GetPropertyByIdAsync(id);
            return property == null ? null : MapToDto(property);
        }

        public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync()
        {
            var properties = await _propertyRepository.GetAllPropertiesAsync();
            return properties.Select(MapToDto);
        }

        private static PropertyDto MapToDto(Property property)
        {
            return new PropertyDto
            {
                Name = property.Name,
                Address = property.Address,
                Price = property.Price,
                CodeInternal = property.CodeInternal,
                Year = property.Year,
                Owner = property.Owner == null ? null : new OwnerDto
                {
                    Name = property.Owner.Name,
                    Address = property.Owner.Address,
                    Photo = property.Owner.Photo,
                    Birthday = property.Owner.Birthday
                },
                Images = property.Images.Select(image => new ImageDto
                {
                    File = image.File,
                    Enabled = image.Enabled
                }),
                Traces = property.Traces.Select(trace => new TraceDto
                {
                    DateSale = trace.DateSale,
                    Name = trace.Name,
                    Value = trace.Value,
                    Tax = trace.Tax
                })
            };
        }
    }
}


using AutoMapper;
using PropertEase.Application.Dtos;
using PropertEase.Domain.Entities;

namespace PropertEase.Application.Mapping
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<Property, PropertyDto>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Traces, opt => opt.MapFrom(src => src.Traces));

            CreateMap<Owner, OwnerDto>();
            CreateMap<PropertyImage, ImageDto>();
            CreateMap<PropertyTrace, TraceDto>();
        }
    }
}

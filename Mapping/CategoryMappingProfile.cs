using AutoMapper;
using ErpConnector.DTOs;
using ErpConnector.Models;

namespace ErpConnector.Mapping
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CategoryFromApiDto, Category>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ApiId, opt => opt.MapFrom(src => src.Slug));
        }
    }
}

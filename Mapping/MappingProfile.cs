using AutoMapper;
using ErpConnector.DTOs;
using ErpConnector.Models;

namespace ErpConnector.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductFromApiDto, Product>()
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.ApiId, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.FullDescription, opt => opt.MapFrom(src => src.Description));


            CreateMap<CategoryFromApiDto, Category>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ApiId, opt => opt.MapFrom(src => src.Slug));
        }
    }
}
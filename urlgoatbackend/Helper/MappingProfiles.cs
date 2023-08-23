using AutoMapper;
using urlgoatbackend.Dto;
using urlgoatbackend.Models;

namespace urlgoatbackend.Helper
{
    // Although my API is simple, and AutoMapper isn't necessarily needed,
    // I'm adding it to maintain best software design practices and for scalability.
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Configure the mapping from CreateShortUrlDto to UrlMapping
            CreateMap<CreateShortUrlDto, UrlMapping>()
                .ForMember(dest => dest.LongUrl, opt => opt.MapFrom(src => src.LongUrl))
                .ReverseMap(); // Configure the reverse mapping from UrlMapping to CreateShortUrlDto

            // Configure the mapping from CreateShortUrl to its Dto
            CreateMap<CreateShortUrl, CreateShortUrlDto>().ReverseMap();
        }
    }
}

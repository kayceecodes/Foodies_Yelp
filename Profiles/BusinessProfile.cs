using foodies_yelp.Models.Dtos.Responses.Yelp;
using AutoMapper;

namespace foodies_yelp.Profiles.BusinessProfile;

public class BusinessProfile : Profile
{
    public BusinessProfile() 
    {
        CreateMap<Business, GetBusinessResponse>()
        .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
        .ForMember(dest => dest.Alias, src => src.MapFrom(x => x.Alias))
        .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
        .ForMember(dest => dest.URL, src => src.MapFrom(x => x.Url))
        .ForMember(dest => dest.Rating, src => src.MapFrom(x => x.Rating))
        .ForMember(dest => dest.ReviewCount, src => src.MapFrom(x => x.Review_count))
        .ForMember(dest => dest.Categories, src => src.MapFrom(x => x.Categories))
        .ForMember(dest => dest.Latitude, src => src.MapFrom(x => x.Coordinates.Latitude))
        .ForMember(dest => dest.Longitude, src => src.MapFrom(x => x.Coordinates.Longitude))
        .ForMember(dest => dest.StreetAddress, src => src.MapFrom(x => x.Location.Address1))
        .ForMember(dest => dest.City, src => src.MapFrom(x => x.Location.City))
        .ForMember(dest => dest.State, src => src.MapFrom(x => x.Location.State))
        .ForMember(dest => dest.ZipCode, src => src.MapFrom(x => x.Location.Zip_code))
        .ForMember(dest => dest.Price, src => src.MapFrom(x => x.Price));
    }
}

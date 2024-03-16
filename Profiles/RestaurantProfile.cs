using foodies_yelp.Models.Dtos.Responses.Yelp;
using AutoMapper;

namespace foodies_yelp.Profiles.RestaurantProfile;

public class RestaurantProfile : Profile
{
    public RestaurantProfile() 
    {
        CreateMap<Business, GetRestaurantResponse>()
        .ForMember(dest => dest.StreetAddress, src => src.MapFrom(x => x.Location.Address1))
        .ForMember(dest => dest.City, src => src.MapFrom(x => x.Location.City))
        .ForMember(dest => dest.State, src => src.MapFrom(x => x.Location.State))
        .ForMember(dest => dest.ZipCode, src => src.MapFrom(x => x.Location.Zip_code));

        CreateMap<Review, GetReviewResponse>()
        .ForMember(dest => dest.UsersName, src => src.MapFrom(x => x.User.Name))
        .ForMember(dest => dest.Text, src => src.MapFrom(x => x.Text));
    }
}

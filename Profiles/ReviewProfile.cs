using AutoMapper;
using foodies_yelp.Models.Dtos.Responses.Yelp;

namespace foodies_yelp;

public class ReviewProfile : Profile
{
    public ReviewProfile() 
    {
        CreateMap<Review, GetReviewResponse>()
            .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
            .ForMember(dest => dest.Text, src => src.MapFrom(x => x.Text))
            .ForMember(dest => dest.Url, src => src.MapFrom(x => x.Url))
            .ForMember(dest => dest.Rating, src => src.MapFrom(x => x.Rating))
            .ForMember(dest => dest.TimeCreated, src => src.MapFrom(x => x.TimeCreated));
        
        CreateMap<User, GetReviewResponse>()
            .ForMember(dest => dest.UsersName, src => src.MapFrom(x => x.Name))
            .ForMember(dest => dest.ImageUrl, src => src.MapFrom(x => x.ImageUrl));
    }
}

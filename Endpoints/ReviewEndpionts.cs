using AutoMapper;
using foodies_yelp.Models.Dtos.Responses.Yelp;
using foodies_yelp.Services;
using Microsoft.AspNetCore.Mvc;

namespace foodies_yelp.Endpoints;

public static class ReviewEndpionts
{
    public static void ConfigurationReviewEndpoints(this WebApplication app) 
    {
        app.MapGet("/api/restaurant/{id}/reviews/", async Task<IResult> ([FromServices] IMapper mapper, string id) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            var result = await YelpService.GetReviewsById(id);
            List<Review> reviews = result.Data as List<Review>;

            if (result.IsSuccess)
            {
                var mappedReviews = result.Data.Select(x => mapper.Map<GetReviewResponse>(x)).ToList();
                var mappedUsers = reviews.Select(x => mapper.Map<GetReviewResponse>(x.User)).ToList();
                                        // Combine lists
                var mapped = (from review in mappedReviews
                                    join user in mappedUsers on review.Id equals user.Id
                                    select new GetReviewResponse
                                    {
                                        Id = review.Id,
                                        UserName = user.UserName,
                                        Rating = review.Rating,
                                        Text = review.Text,
                                        TimeCreated = review.TimeCreated,
                                        Url = review.Url
                                    }).ToList();


                return TypedResults.Ok(mapped);
            }
                
            return TypedResults.BadRequest();
        
        }).WithName("GetReviewById").Accepts<string>("application/json")
        .Produces<List<Review>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        // app.MapPost("/api/review", async () => {}).WithName("AddReview").Accepts<ReviewDto>("application/json")
        // .Produces<List<ReviewDto>>(StatusCodes.Status200OK)
        // .Produces(StatusCodes.Status400BadRequest);
    }
}

using AutoMapper;
using foodies_yelp.Models.Dtos.Responses.Yelp;
using foodies_yelp.Services;
using Microsoft.AspNetCore.Mvc;

namespace foodies_yelp.Endpoints;

public static class ReviewEndpionts
{
    public static void ConfigurationReviewEndpoints(this WebApplication app) 
    {
        app.MapGet("/api/restaurant/{id}/reviews/", async Task<IResult> (HttpContext context, [FromServices] IMapper mapper, string id) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            var result = await YelpService.GetReviewsById(id);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Data);
                
            return TypedResults.BadRequest();
        
        }).WithName("GetReviewById").Accepts<string>("application/json")
        .Produces<List<Review>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        // app.MapPost("/api/review", async () => {}).WithName("AddReview").Accepts<ReviewDto>("application/json")
        // .Produces<List<ReviewDto>>(StatusCodes.Status200OK)
        // .Produces(StatusCodes.Status400BadRequest);
    }
}

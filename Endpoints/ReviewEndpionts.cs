using foodies_yelp.Models.Dtos;
using foodies_yelp.Services;

namespace foodies_yelp.Endpoints;

public static class ReviewEndpionts
{
    public static void ConfigurationReviewEndpoints(this WebApplication app) 
    {
        app.MapGet("/api/restaurant/{id}/reviews/", async Task<IResult> (HttpContext context, string id) =>
        {
            var YelpApiClient = app.Services.GetRequiredService<YelpApiClient>();
            var result = await YelpApiClient.GetReviewsById(id);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Data);
            else
                return TypedResults.BadRequest();
        
        }).WithName("GetReviewById").Accepts<string>("application/json")
        .Produces<List<Review>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        // app.MapPost("/api/review", async () => {}).WithName("AddReview").Accepts<ReviewDto>("application/json")
        // .Produces<List<ReviewDto>>(StatusCodes.Status200OK)
        // .Produces(StatusCodes.Status400BadRequest);
    }
}

using foodies_yelp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using foodies_yelp.Models.Dtos.Responses;
using foodies_yelp.Services;
using foodies_yelp.Models.Dtos.Responses.Yelp;


namespace foodies_yelp.Endpoints;

public static class RestaurantEndpoints
{
    public static void ConfigurationRestaurantEndpoints(this WebApplication app) 
    {
        app.MapGet("/api/restaurant/name/{name}/location/{location}", async Task<IResult> (HttpContext context, string name, string location) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            APIResult<List<Business>> result = await YelpService.GetBusinessesByName(name, location);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Data);
            else
                return TypedResults.BadRequest();
        
        }).WithName("GetRestaurantByNameAndLocation").Accepts<string>("application/json")
        .Produces<APIResult<List<Business>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("/api/restaurant/{id}", async Task<IResult> (HttpContext context, string id) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            APIResult<Business> result = await YelpService.GetBusinessById(id);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Data);
            else
                return TypedResults.BadRequest();
        
        }).WithName("GetRestaurantById").Accepts<string>("application/json")
        .Produces<APIResult<Business>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
        
        app.MapGet("/api/restaurant/location/{location}", async Task<IResult> (HttpContext context, string location) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            APIResult<List<Business>> result = await YelpService.GetBusinessesByLocation(location);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Data);
            else
                return TypedResults.BadRequest();
        
        }).WithName("GetRestaurantByLocation").Accepts<string>("application/json")
        .Produces<APIResult<List<Business>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
        
        app.MapGet("/api/restaurant/phone/{phonenumber}", async Task<IResult> (HttpContext context, string phonenumber) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            APIResult<Business> result = await YelpService.GetBusinessByPhone(phonenumber);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Data);
            else
                return TypedResults.BadRequest();
        
        }).WithName("GetRestaurantByPhone").Accepts<string>("application/json")
        .Produces<APIResult<Business>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        // Uses Search object with propeerties used in Yelp's API
        app.MapPost("/api/restaurant/search/", async Task<IResult> (HttpContext context, [FromBody] SearchDto search) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            APIResult<List<Business>> result = await YelpService.GetBusinesses(search);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Data);
            else
                return TypedResults.BadRequest();
        
        }).WithName("GetRestaurantsBySearchTerms").Accepts<SearchDto>("application/json")
        .Produces<APIResult<List<Business>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
 
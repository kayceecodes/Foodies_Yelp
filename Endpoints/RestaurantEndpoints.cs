using foodies_yelp.Models.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;
using foodies_yelp.Models.Dtos.Responses;
using foodies_yelp.Services;
using foodies_yelp.Models.Dtos.Responses.Yelp;
using AutoMapper;


namespace foodies_yelp.Endpoints;

public static class RestaurantEndpoints
{
    public static void ConfigurationRestaurantEndpoints(this WebApplication app) 
    {
        app.MapGet("/api/restaurant/name/{name}/location/{location}", async Task<IResult> (HttpContext context, [FromServices] IMapper mapper, string name, string location) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            APIResult<List<Business>> result = await YelpService.GetBusinessesByName(name, location);

            if (result.IsSuccess)
            {
                var mapped = result.Data.Select(x => mapper.Map<GetRestaurantResponse>(x)).ToList();
                return TypedResults.Ok(mapped);
            }
                
            return TypedResults.BadRequest();

        }).WithName("GetRestaurantByNameAndLocation").Accepts<string>("application/json")
        .Produces<APIResult<List<Business>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("/api/restaurant/{id}", async Task<IResult> (HttpContext context, [FromServices] IMapper mapper, string id) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            APIResult<Business> result = await YelpService.GetBusinessById(id);

            if (result.IsSuccess)
            {
                var mapped = mapper.Map<GetRestaurantResponse>(result.Data);
                return TypedResults.Ok(mapped);
            }
                
            return TypedResults.BadRequest();
        
        }).WithName("GetRestaurantById").Accepts<string>("application/json")
        .Produces<APIResult<Business>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
        
        app.MapGet("/api/restaurant/location/{location}", async Task<IResult> (HttpContext context, [FromServices] IMapper mapper, string location) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            APIResult<List<Business>> result = await YelpService.GetBusinessesByLocation(location);

            if (result.IsSuccess)
            {
                var mapped = result.Data.Select(x => mapper.Map<GetRestaurantResponse>(x)).ToList();
                return TypedResults.Ok(mapped);
            }
                
            return TypedResults.BadRequest();

        
        }).WithName("GetRestaurantByLocation").Accepts<string>("application/json")
        .Produces<APIResult<List<Business>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
        
        app.MapGet("/api/restaurant/phone/{phonenumber}", async Task<IResult> (HttpContext context, [FromServices] IMapper mapper, string phonenumber) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            APIResult<Business> result = await YelpService.GetBusinessByPhone(phonenumber);

            if (result.IsSuccess)
            {
                var mapped = mapper.Map<GetRestaurantResponse>(result.Data);
                return TypedResults.Ok(result.Data);
            }

            return TypedResults.BadRequest();
        
        }).WithName("GetRestaurantByPhone").Accepts<string>("application/json")
        .Produces<APIResult<Business>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        // Uses Search object with propeerties used in Yelp's API
        app.MapPost("/api/restaurant/search/", async Task<IResult> (HttpContext context, [FromServices] IMapper mapper, [FromBody] SearchDto search) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            APIResult<List<Business>> result = await YelpService.GetBusinessesByKeywords(search);

            if (result.IsSuccess)
            {
                var mapped = result.Data.Select(x => mapper.Map<GetRestaurantResponse>(x)).ToList();
                return TypedResults.Ok(mapped);
            }
                
            return TypedResults.BadRequest();

        
        }).WithName("GetRestaurantsBySearchTerms").Accepts<SearchDto>("application/json")
        .Produces<APIResult<List<Business>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
 
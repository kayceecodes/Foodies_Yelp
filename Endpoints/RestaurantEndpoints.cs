using foodies_yelp.Models.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using foodies_yelp.Models.Responses;
using foodies_yelp.Services;


namespace foodies_yelp.Endpoints;

public static class RestaurantEndpoints
{
    public static void ConfigurationRestaurantEndpoints(this WebApplication app) 
    {
        app.MapGet("/api/restaurant/{id}", async Task<IResult> (HttpContext context, string id) =>
        {
            var YelpApiClient = app.Services.GetRequiredService<YelpApiClient>();
            APIResult result = await YelpApiClient.GetBusinessById(id);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Data);
            else
                return TypedResults.BadRequest();
        
        }).WithName("GetRestaurantById").Accepts<RestaurantDto>("application/json")
        .Produces<APIResult>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("/api/restaurant/location/{location}", async Task<IResult> (HttpContext context, string location) =>
        {
            var YelpApiClient = app.Services.GetRequiredService<YelpApiClient>();
            APIResult result = await YelpApiClient.GetBusinessesByLocation(location);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Data);
            else
                return TypedResults.BadRequest();
        
        }).WithName("GetRestaurantByLocation").Accepts<List<RestaurantDto>>("application/json")
        .Produces<APIResult>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
        
        app.MapGet("/api/restaurant/phone/{phonenumber}", async Task<IResult> (HttpContext context, string phonenumber) =>
        {
            var YelpApiClient = app.Services.GetRequiredService<YelpApiClient>();
            APIResult result = await YelpApiClient.GetBusinessByPhone(phonenumber);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Data);
            else
                return TypedResults.BadRequest();
        
        }).WithName("GetRestaurantByPhone").Accepts<List<RestaurantDto>>("application/json")
        .Produces<APIResult>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        // Uses Search object with propeerties used in Yelp's API
        app.MapPost("/api/restaurant/search/", async Task<IResult> (HttpContext context, [FromBody] SearchDto search) =>
        {
            var YelpApiClient = app.Services.GetRequiredService<YelpApiClient>();
            APIResult result = await YelpApiClient.GetBusinesses(search);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Data);
            else
                return TypedResults.BadRequest();
        
        }).WithName("GetRestaurantsBySearchTerms").Accepts<SearchDto>("application/json")
        .Produces<APIResult>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
 
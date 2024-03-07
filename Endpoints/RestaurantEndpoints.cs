﻿using foodies_yelp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using foodies_yelp.Models.Responses;
using foodies_yelp.Services;
using foodies_yelp.Models.Responses.Yelp;


namespace foodies_yelp.Endpoints;

public static class RestaurantEndpoints
{
    public static void ConfigurationRestaurantEndpoints(this WebApplication app) 
    {
        app.MapGet("/api/restaurant/{id}", async Task<IResult> (HttpContext context, string id) =>
        {
            var YelpApiClient = app.Services.GetRequiredService<YelpApiClient>();
            APIResult<Business> result = await YelpApiClient.GetBusinessById(id);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Data);
            else
                return TypedResults.BadRequest();
        
        }).WithName("GetRestaurantById").Accepts<string>("application/json")
        .Produces<APIResult<Business>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
        
        app.MapGet("/api/restaurant/location/{location}", async Task<IResult> (HttpContext context, string location) =>
        {
            var YelpApiClient = app.Services.GetRequiredService<YelpApiClient>();
            APIResult<List<Business>> result = await YelpApiClient.GetBusinessesByLocation(location);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Data);
            else
                return TypedResults.BadRequest();
        
        }).WithName("GetRestaurantByLocation").Accepts<string>("application/json")
        .Produces<APIResult<List<Business>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
        
        app.MapGet("/api/restaurant/phone/{phonenumber}", async Task<IResult> (HttpContext context, string phonenumber) =>
        {
            var YelpApiClient = app.Services.GetRequiredService<YelpApiClient>();
            APIResult<Business> result = await YelpApiClient.GetBusinessByPhone(phonenumber);

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
            var YelpApiClient = app.Services.GetRequiredService<YelpApiClient>();
            APIResult<List<Business>> result = await YelpApiClient.GetBusinesses(search);

            if (result.IsSuccess)
                return TypedResults.Ok(result.Data);
            else
                return TypedResults.BadRequest();
        
        }).WithName("GetRestaurantsBySearchTerms").Accepts<SearchDto>("application/json")
        .Produces<APIResult<List<Business>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
 
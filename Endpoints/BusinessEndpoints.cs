using foodies_yelp.Models.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;
using foodies_yelp.Models.Dtos.Responses;
using foodies_yelp.Services;
using foodies_yelp.Models.Dtos.Responses.Yelp;
using AutoMapper;


namespace foodies_yelp.Endpoints;

public static class BusinessEndpoints
{
    public static void ConfigurationBusinessEndpoints(this WebApplication app) 
    {
        app.MapGet("/api/business/name/{name}/location/{location}", async Task<IResult> ([FromServices] IMapper mapper, string name, string location) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            APIResult<List<Business>> result = await YelpService.GetBusinessesByName(name, location);

            if (result.IsSuccess)
            {
                var mapped = result.Data.Select(x => mapper.Map<GetBusinessResponse>(x)).ToList();
                return TypedResults.Ok(mapped);
            }
                
            return TypedResults.BadRequest(result.ErrorMessages);

        }).WithName("GetBusinessByNameAndLocation").Accepts<string>("application/json")
        .Produces<APIResult<List<Business>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("/api/business/{id}", async Task<IResult> ([FromServices] IMapper mapper, string id) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            APIResult<Business> result = await YelpService.GetBusinessById(id);

            if (result.IsSuccess)
            {
                var mapped = mapper.Map<GetBusinessResponse>(result.Data);
                return TypedResults.Ok(mapped);
            }
                
            return TypedResults.BadRequest("Error at 'GetBusinessById'");
        
        }).WithName("GetBusinessById").Accepts<string>("application/json")
        .Produces<APIResult<Business>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
        
        app.MapGet("/api/business/location/{location}", async Task<IResult> ([FromServices] IMapper mapper, string location) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            APIResult<List<Business>> result = await YelpService.GetBusinessesByLocation(location);

            if (result.IsSuccess)
            {
                var mapped = result.Data.Select(x => mapper.Map<GetBusinessResponse>(x)).ToList();
                return TypedResults.Ok(mapped);
            }
                
            return TypedResults.BadRequest(result.ErrorMessages);

        
        }).WithName("GetBusinessByLocation").Accepts<string>("application/json")
        .Produces<APIResult<List<Business>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
        
        app.MapGet("/api/business/phone/{phonenumber}", async Task<IResult> ([FromServices] IMapper mapper, string phonenumber) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            APIResult<Business> result = await YelpService.GetBusinessByPhone(phonenumber);

            if (result.IsSuccess)
            {
                var mapped = mapper.Map<GetBusinessResponse>(result.Data);
                return TypedResults.Ok(result.Data);
            }

            return TypedResults.BadRequest(result.ErrorMessages);
        
        }).WithName("GetBusinessByPhone").Accepts<string>("application/json")
        .Produces<APIResult<Business>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        // Uses Search object with propeerties used in Yelp's API
        app.MapPost("/api/business/search/", async Task<IResult> ([FromServices] IMapper mapper, [FromBody] SearchDto search) =>
        {
            var YelpService = app.Services.GetRequiredService<YelpService>();
            APIResult<List<Business>> result = await YelpService.GetBusinessesByKeywords(search);

            if (result.IsSuccess)
            {
                var mapped = result.Data.Select(x => mapper.Map<GetBusinessResponse>(x)).ToList();
                return TypedResults.Ok(mapped);
            }
                
            return TypedResults.BadRequest(result.ErrorMessages);

        
        }).WithName("GetBusinesssBySearchTerms").Accepts<SearchDto>("application/json")
        .Produces<APIResult<List<Business>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
 
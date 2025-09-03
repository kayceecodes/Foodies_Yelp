using Newtonsoft.Json;
using System.Net.Http.Headers;
using foodies_yelp.Models.Dtos.Requests;
using foodies_yelp.Models.Dtos.Responses;
using foodies_yelp.Models.Dtos.Responses.Yelp;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace foodies_yelp.Services;

public class YelpService : IYelpService
{
    private ILogger<YelpService> _logger; 
    private IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public YelpService(ILogger<YelpService> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    private bool IsPlaceHolderString(string token)
    {
        string pattern = @"(API|SECRET|TOKEN)";
        
        Regex regex = new Regex(pattern);
        MatchCollection matches = regex.Matches(token);

        return matches.Count > 0;
    }

    public HttpClient CreateClient() 
    {
        var client = _httpClientFactory.CreateClient("YelpService");
        var apiKey = string.Empty;
        
        try {
            if (File.Exists("/run/secrets/yelp-api-key"))
                apiKey = File.ReadAllText("/run/secrets/yelp-api-key").Trim();
            else
                apiKey = _configuration["YELP_API_KEY"];

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }
        catch(UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Cannot reach API Key");
            throw new UnauthorizedAccessException("Cannot reach API Key");
        }
        
        if(apiKey.IsNullOrEmpty())
        {
            _logger.LogError("API Key is empty");
            throw new UnauthorizedAccessException("API Key is empty");
        }

        return client;
    }

    public async virtual Task<APIResult<Business>> GetBusinessById(string id)
    {
        HttpClient client = CreateClient();
        string url = client.BaseAddress + $"/businesses/{id}";

        HttpResponseMessage result = await client.GetAsync(url);
        var business = JsonConvert.DeserializeObject<Business>(await result.Content.ReadAsStringAsync());

        if (result.IsSuccessStatusCode)
            return APIResult<Business>.Pass(business);
        else
            return APIResult<Business>.Fail($"Problem getting bussiness using Id: {id}", result.StatusCode);
    }

    public async Task<APIResult<List<Business>>> GetBusinessesByName(string name, string location)
    {
        HttpClient client = CreateClient();
        string url = client.BaseAddress + $"/businesses/search?sort_by=best_match&limit=10&term={name}&location={location}";

        if(name.IsNullOrEmpty())
            throw new Exception("Name is missing in method YelpService.GetBusinessesByName");

        if(location.IsNullOrEmpty())
            throw new Exception("Location is missing in method YelpService.GetBusinessesByName");

        HttpResponseMessage result = await client.GetAsync(url);
        var yelpResponse = JsonConvert.DeserializeObject<YelpResponse>(await result.Content.ReadAsStringAsync());
        List<Business> businesses = yelpResponse?.Businesses ?? new ();

        if (result.IsSuccessStatusCode)
            return APIResult<List<Business>>.Pass(businesses);
        else
           return APIResult<List<Business>>.Fail($"Problem getting bussinesses using name: {name}, location: {location}", result.StatusCode);
    }

    public async Task<APIResult<List<Business>>> GetBusinessesByLocation(string location)
    {
        HttpClient client = CreateClient();
        string url = client.BaseAddress + $"/businesses/search?term=food&sort_by=best_match&limit=20&location={location}";

        HttpResponseMessage result = await client.GetAsync(url);
        var yelpResponse = JsonConvert.DeserializeObject<YelpResponse>(await result.Content.ReadAsStringAsync());
        List<Business> businesses = yelpResponse?.Businesses ?? new ();

        if (result.IsSuccessStatusCode)
            return APIResult<List<Business>>.Pass(businesses);
        else
            return APIResult<List<Business>>.Fail($"Problem getting bussinesses using location: {location}", result.StatusCode);
    }

    public async virtual Task<APIResult<Business>> GetBusinessByPhone(string number)
    {
        HttpClient client = CreateClient();
        string url = client.BaseAddress + $"/businesses/search/phone?sort_by=best_match&phone={number}";

        HttpResponseMessage result = await client.GetAsync(url);
        var yelpResponse = JsonConvert.DeserializeObject<YelpResponse>(await result.Content.ReadAsStringAsync());
        var business = yelpResponse?.Businesses?[0];
        
        if (result.IsSuccessStatusCode)
            return APIResult<Business>.Pass(business);
        else
            return APIResult<Business>.Fail($"Problem getting bussiness using phone number: {number}", result.StatusCode);
    }

    public async virtual Task<APIResult<List<Business>>> GetBusinessesByKeywords(SearchDto dto)
    {
        string terms = "food, dinner, restaurant";
        
        if(dto.Terms.Count > 0)
            terms += ", " + string.Join(", ", dto.Terms);    

        bool IsMissingCoordinates = dto.Lat.IsNullOrEmpty() || dto.Long.IsNullOrEmpty();

        if (IsMissingCoordinates && dto.Location.IsNullOrEmpty())
            throw new NullReferenceException("There are no values for the following variables: Lat, Long, & Location.");  
        
        string endpoint ="/businesses/search"; 
        var query = $"?sort_by=best_match&limit={dto.Limit}&term={terms}&location={dto.Location}&latitude={dto.Lat}&longitude={dto.Long}";
        
        HttpClient client = CreateClient();

        string url = client.BaseAddress + endpoint + query;
        HttpResponseMessage result = await client.GetAsync(url);
        var yelpResponse = JsonConvert.DeserializeObject<YelpResponse>(await result.Content.ReadAsStringAsync());
        List<Business> businesses = yelpResponse?.Businesses ?? new ();

        if (result.IsSuccessStatusCode)
            return APIResult<List<Business>>.Pass(businesses);
        else
            return APIResult<List<Business>>.Fail("Problem getting bussinesses by Keywords", result.StatusCode);
    }

    public async Task<APIResult<List<Review>>> GetReviewsById(string id)
    {
        HttpClient client = CreateClient();
        string url = client.BaseAddress + $"/businesses/{id}/reviews";

        HttpResponseMessage result = await client.GetAsync(url);
        var response = JsonConvert.DeserializeObject<YelpResponse>(await result.Content.ReadAsStringAsync());
        var reviews = response?.Reviews ?? new ();
        
        if (result.IsSuccessStatusCode)
            return APIResult<List<Review>>.Pass(reviews);
        else
            return APIResult<List<Review>>.Fail($"Problem getting Reviews by Id: {id}", result.StatusCode);
    }
}
using Newtonsoft.Json;
using System.Net.Http.Headers;
using foodies_yelp.Models.Dtos.Requests;
using foodies_yelp.Models.Dtos.Responses;
using foodies_yelp.Models.Dtos.Responses.Yelp;
using Microsoft.IdentityModel.Tokens;
using foodies_yelp.Models.Options;
using Microsoft.Extensions.Options;

namespace foodies_yelp.Services;

public class YelpService : IYelpService
{
    private ILogger<YelpService> _logger; 
    private IHttpClientFactory _httpClientFactory;
    private YelpOptions _yelpOptions;
    public YelpService(ILogger<YelpService> logger, IHttpClientFactory httpClientFactory, IOptions<YelpOptions> yelpOptions)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _yelpOptions = yelpOptions.Value;
    }

    public HttpClient CreateClient() 
    {
        // var token = _configuration.GetValue<string>(YelpConstants.ApiKeyName);
        var client = _httpClientFactory.CreateClient("YelpService");
        try {
            var token = _yelpOptions.ApiKey;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        catch(Exception ex) {
            _logger.LogError(ex, "API token is missing.");
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
            return APIResult<Business>.Fail("Problem getting bussiness", result.StatusCode);
    }

    public async Task<APIResult<List<Business>>> GetBusinessesByName(string name, string location)
    {
        HttpClient client = CreateClient();
        string url = client.BaseAddress + $"/businesses/search?sort_by=best_match&limit=10&term={name}&location={location}";

        if(name.IsNullOrEmpty())
            throw new Exception("Name is missing");

        if(location.IsNullOrEmpty())
            throw new Exception("Location is missing");

        HttpResponseMessage result = await client.GetAsync(url);
        var yelpResponse = JsonConvert.DeserializeObject<YelpResponse>(await result.Content.ReadAsStringAsync());
        List<Business> businesses = yelpResponse?.Businesses ?? new ();

        if (result.IsSuccessStatusCode)
            return APIResult<List<Business>>.Pass(businesses);
        else
            return APIResult<List<Business>>.Fail("Problem getting bussinesses", result.StatusCode);
    }

    public async Task<APIResult<List<Business>>> GetBusinessesByLocation(string location)
    {
        HttpClient client = CreateClient();
        string url = client.BaseAddress + $"/businesses/search?term=restaurant&sort_by=best_match&limit=20&location={location}";

        HttpResponseMessage result = await client.GetAsync(url);
        var yelpResponse = JsonConvert.DeserializeObject<YelpResponse>(await result.Content.ReadAsStringAsync());
        List<Business> businesses = yelpResponse?.Businesses ?? new ();

        if (result.IsSuccessStatusCode)
            return APIResult<List<Business>>.Pass(businesses);
        else
            return APIResult<List<Business>>.Fail("Problem getting bussinesses", result.StatusCode);
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
            return APIResult<Business>.Fail("Problem getting bussiness", result.StatusCode);
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
            return APIResult<List<Business>>.Fail("Problem getting bussinesses", result.StatusCode);
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
            return APIResult<List<Review>>.Fail("Problem getting bussiness", result.StatusCode);
    }
}
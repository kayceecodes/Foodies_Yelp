using Newtonsoft.Json;
using System.Net.Http.Headers;
using foodies_yelp.Models.Dtos;
using foodies_yelp.Models.Responses;
using foodies_yelp.Models.Responses.Yelp;
using Microsoft.IdentityModel.Tokens;
using foodies_yelp.Models.Options;
using Microsoft.Extensions.Options;

namespace foodies_yelp.Services;

public class YelpApiClient : IYelpApiClient
{
    private ILogger<YelpApiClient> _logger; 
    private IHttpClientFactory _httpClientFactory;
    private IConfiguration _configuration;
    private Yelp _yelp;
    public YelpApiClient(ILogger<YelpApiClient> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration, IOptions<Yelp> yelpOptions)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _yelp = yelpOptions.Value;
    }

    public HttpClient CreateClient() 
    {
        // var token = _configuration.GetValue<string>(YelpConstants.ApiKeyName);
        var client = _httpClientFactory.CreateClient("YelpApiClient");
        try {
            var token = _yelp.Key;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        catch(Exception ex) {
            _logger.LogError(ex, "API token is missing.");
        }
        
        return client;
    }

    public async Task<APIResult<Business>> GetBusinessById(string id)
    {
        HttpClient client = CreateClient();
        string url = client.BaseAddress + $"/businesses/{id}";

        HttpResponseMessage result = await client.GetAsync(url);
        var business = JsonConvert.DeserializeObject<Business>(await result.Content.ReadAsStringAsync());

        if (result.IsSuccessStatusCode)
            return APIResult<Business>.Pass(business ?? new ());
        else
            return APIResult<Business>.Fail("Problem getting bussiness", result.StatusCode);
    }

    public async Task<APIResult<List<Business>>> GetBusinessesByLocation(string location)
    {
        HttpClient client = CreateClient();
        string url = client.BaseAddress + $"/businesses/search?term=restaurant&sort_by=best_match&limit=20&location={location}";

        HttpResponseMessage result = await client.GetAsync(url);
        var yelpResponse = JsonConvert.DeserializeObject<YelpResponse>(await result.Content.ReadAsStringAsync());
        List<Business> businesses = yelpResponse?.Businesses ?? new List<Business>();

        if (result.IsSuccessStatusCode)
            return APIResult<List<Business>>.Pass(businesses ?? new List<Business>());
        else
            return APIResult<List<Business>>.Fail("Problem getting bussinesses", result.StatusCode);
    }

    public async Task<APIResult<Business>> GetBusinessByPhone(string number)
    {
        HttpClient client = CreateClient();
        string url = client.BaseAddress + $"/businesses/search/phone?sort_by=best_match&phone={number}";

        HttpResponseMessage result = await client.GetAsync(url);
        var business = JsonConvert.DeserializeObject<Business>(await result.Content.ReadAsStringAsync());

        if (result.IsSuccessStatusCode)
            return APIResult<Business>.Pass(business ?? new ());
        else
            return APIResult<Business>.Fail("Problem getting bussiness", result.StatusCode);
    }

    public async Task<APIResult<List<Business>>> GetBusinesses(SearchDto dto)
    {
        string terms = "food, dinner, restaurant";
        // if(!dto.Terms.IsNullOrEmpty()) 
        // {
        //     terms = dto.Terms.Count > 1 ? string.Join(", ", dto.Terms) : dto.Terms[0] + ", ";// add split for terms5
        //     terms += "food, restaurant, dinner";
        // }
        if(dto.Terms.Count > 0)
            terms += string.Join(", ", dto.Terms);    

        bool IsMissingLatLong = dto.Lat.IsNullOrEmpty() || dto.Long.IsNullOrEmpty();

        if (IsMissingLatLong && dto.Location.IsNullOrEmpty())
            throw new NullReferenceException("There no value for Lat, Long, or Location");  
        
        string endpoint ="/businesses/search"; 
        var query = $"?sort_by=best_match&limit={dto.Limit}&term={terms}&location={dto.Location}&latitude={dto.Lat}&longitude={dto.Long}&categories={dto.Category}";
        
        HttpClient client = CreateClient();

        string url = client.BaseAddress + endpoint + query;
        HttpResponseMessage result = await client.GetAsync(url);
        var businesses = JsonConvert.DeserializeObject<List<Business>>(await result.Content.ReadAsStringAsync());

        if (result.IsSuccessStatusCode)
            return APIResult<List<Business>>.Pass(businesses ?? new List<Business>());
        else
            return APIResult<List<Business>>.Fail("Problem getting bussinesses", result.StatusCode);
    }

    public async Task<APIResult<List<ReviewDto>>> GetReviewsById(string id)
    {
        HttpClient client = CreateClient();
        string url = client.BaseAddress + $"/businesses/{id}/reviews";

        HttpResponseMessage result = await client.GetAsync(url);
        var reviews = JsonConvert.DeserializeObject<List<ReviewDto>>(await result.Content.ReadAsStringAsync());

        if (result.IsSuccessStatusCode)
            return APIResult<List<ReviewDto>>.Pass(reviews ?? new ());
        else
            return APIResult<List<ReviewDto>>.Fail("Problem getting bussiness", result.StatusCode);
    }
}
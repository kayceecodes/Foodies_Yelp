using foodies_yelp.Services;
using NUnit.Framework;
using Moq;
using foodies_yelp.Models.Dtos.Responses.Yelp;
using foodies_yelp.Models.Dtos.Requests;
using foodies_yelp.Models.Dtos.Responses;
using Microsoft.Extensions.Options;
using foodies_yelp.Models.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;


namespace foodies_yelp.testing;

[TestFixture]
public class YelpServiceTests
{
    private List<Business> _businesses = new List<Business>(3);
    private ILogger<YelpService> mockLogger;
    private IHttpClientFactory mockClientFactory;
    private IConfiguration mockConfiguration;
    private IOptions<Yelp> mockYelpOptions;

    [SetUp]
    public void Setup()
    {
        var service = new Mock<YelpService>();
        CreateBusinesses();
        service.Setup(x => x.GetBusinesses(It.IsAny<SearchDto>()))
            .ReturnsAsync(new APIResult<List<Business>>() 
            { IsSuccess = true, Data = _businesses});
        service.Setup(x => x.GetBusinessByPhone(It.IsAny<string>()));
    }

    private List<Business> CreateBusinesses()
    {
        var count = 0;
        var businesses = _businesses;
        foreach(var business in _businesses) 
        {
            count++;
            businesses.Add(new() {
                Id = count.ToString(),
                Name = "business #" + count,
                Rating = count,
                Price = "$25.99"
            });
        }

        return businesses;
    }

    private YelpService CreateYelpService()
    {
        var service = new YelpService(mockLogger, mockClientFactory, mockConfiguration, mockYelpOptions);
        return service;
    }

    [Test]
    public void GetBusinessById()
    {
        var service = CreateYelpService();
        var result = service.GetBusinessById("1");
        
        string id = result.Result.Data.Id;

        Assert.Equals("1", id);
    }
}
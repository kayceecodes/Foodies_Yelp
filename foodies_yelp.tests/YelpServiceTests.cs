using foodies_yelp.Services;
using NUnit.Framework;
using Moq;
using Moq.Protected;
using foodies_yelp.Models.Dtos.Responses.Yelp;
using foodies_yelp.Models.Dtos.Requests;
using foodies_yelp.Models.Dtos.Responses;
using Microsoft.Extensions.Options;
using foodies_yelp.Models.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;


namespace foodies_yelp.testing;

[TestFixture]
public class YelpServiceTests
{
    private List<Business> _businesses = new List<Business>(3);
    private ILogger<YelpService> _mockLogger;
    private Mock<IHttpClientFactory> _mockHttpClientFactory;
    private IConfiguration _mockConfiguration;
    private IOptions<Yelp> _mockYelpOptions;

    [SetUp]
    public void Setup()
    {
        var service = new Mock<YelpService>();
        CreateBusinesses();
        CreateMockHttpClient();
        var mockSearchDto = new Mock<SearchDto>();
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();
        var factory = serviceProvider.GetService<ILoggerFactory>();
        _mockLogger = factory.CreateLogger<YelpService>();

        service.Setup(x => x.GetBusinessById(It.IsAny<string>()))
            .ReturnsAsync(new APIResult<Business>()
            { IsSuccess = true, Data = _businesses[0] });
        // service.Setup(x => x.GetBusinesses(It.IsAny<SearchDto>()))
        //     .ReturnsAsync(new APIResult<List<Business>>() 
        //     { IsSuccess = true, Data = _businesses});
        service.Setup(x => x.GetBusinessByPhone(It.IsAny<string>()))
            .ReturnsAsync(new APIResult<Business>()
            { IsSuccess = true, Data = _businesses[0] });
    
        var settings = new Dictionary<string, string>()
        {
            {"Yelp:Key", "TestKey" }
        };
        _mockConfiguration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();

    }

    private void CreateMockHttpClient()
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        HttpResponseMessage result = new HttpResponseMessage();

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(result)
            .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object) {
                BaseAddress = new Uri("https://api.yelp.com/v3")
            };

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();

        _mockHttpClientFactory.Setup(_ => _.CreateClient("YelpService")).Returns(httpClient);        
    }

    private List<Business> CreateBusinesses()
    {
        var count = 0;
        var businesses = _businesses;
        
        for (var i = 0; i < 3; i++)
            businesses.Add(new()
            {
                Id = count.ToString(),
                Name = "business #" + count,
                Rating = count,
                Price = "$25.99"
            });

        return businesses;
    }

    private YelpService CreateYelpService()
    {
        var service = new YelpService(_mockLogger, _mockHttpClientFactory.Object, _mockConfiguration, _mockYelpOptions);
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

    [Test]
    public void GetBusinessByPhone()
    {
        var service = CreateYelpService();
        var result = service.GetBusinessByPhone("555 555 5555");

        Assert.Equals("SomeName", result.Result.Data.Name);
    }
}
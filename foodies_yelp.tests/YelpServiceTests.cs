using foodies_yelp.Services;
using Moq;
using Moq.Protected;
using foodies_yelp.Models.Dtos.Responses.Yelp;
using foodies_yelp.Models.Dtos.Requests;
using Microsoft.Extensions.Options;
using foodies_yelp.Models.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace foodies_yelp.testing;

[TestFixture]
public class YelpServiceTests
{
    private List<Business> _businesses = new List<Business>(3);
    private ILogger<YelpService> _mockLogger;
    private Mock<IHttpClientFactory> _mockHttpClientFactory;
    private IConfiguration _mockConfiguration;
    private Mock<IOptions<Yelp>> _mockYelpOptions;

    [SetUp]
    public void Setup()
    {
        var service = new Mock<YelpService>();
        CreateBusinesses();
        CreateMockHttpClient();
        _mockYelpOptions = new Mock<IOptions<Yelp>>();
        var mockSearchDto = new Mock<SearchDto>();
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();
        var factory = serviceProvider.GetService<ILoggerFactory>();
        _mockLogger = factory.CreateLogger<YelpService>();
    
        var settings = new Dictionary<string, string>()
        {
            {"Yelp:Key", "TestKey" },
            {"Key", "TestKey" }
        };
        _mockConfiguration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        _mockYelpOptions.Setup(m => m.Value).Returns(new Yelp { Key = "Test Key" });
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

        _mockHttpClientFactory = new Mock<IHttpClientFactory>();

        _mockHttpClientFactory.Setup(_ => _.CreateClient("YelpService")).Returns(httpClient);        
    }

    private void CreateBusinesses()
    {
        var businesses = new List<Business>();

        for (var i = 0; i < 3; i++)
            businesses.Add(new()
            {
                Id = i.ToString(),
                Name = "business #" + i,
                Rating = i,
                Price = "$25.99"
            });

        _businesses = businesses;
    }

    private YelpService CreateYelpService()
    {
        var service = new YelpService(_mockLogger, _mockHttpClientFactory.Object, _mockConfiguration, _mockYelpOptions.Object);
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
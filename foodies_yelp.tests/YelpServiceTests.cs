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
using Newtonsoft.Json;
using System.Net;

namespace foodies_yelp.testing;

[TestFixture]
public class YelpServiceTests
{
    private List<Business> _businesses = new List<Business>(3);
    private List<Review> _reviews = new List<Review>(3);
    private ILogger<YelpService> _mockLogger;
    private Mock<IHttpClientFactory> _mockHttpClientFactory;
    private IConfiguration _mockConfiguration;
    private Mock<IOptions<Yelp>> _mockYelpOptions;

    [SetUp]
    public void Setup()
    {
        CreateBusinesses();
        CreateReviews();
        var service = new Mock<YelpService>();
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

    private void CreateMockHttpClient(YelpResponse yelpResponse = null!)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        StringContent httpContent;
        
        if(yelpResponse != null)
        {
            httpContent = new StringContent(
               JsonConvert.SerializeObject(yelpResponse)
            );
        }
        else {
                httpContent = new StringContent(
                    JsonConvert.SerializeObject(this._businesses[0])
                );
        }

        HttpResponseMessage result = new HttpResponseMessage
        {
            Content = httpContent, 
            StatusCode = HttpStatusCode.OK
        };

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

    /// <summary>
    /// Creates a response object that contains multiple businesses
    /// </summary>
    private YelpResponse CreateYelpResponse()
    {
        var response = new YelpResponse()
        {
            Businesses = _businesses,
            Reviews = _reviews
        };

        return response;
    }

    /// <summary>
    /// Creates a single business object
    /// </summary>
    private void CreateBusinesses()
    {
        _businesses = new List<Business>();
        
        for (var i = 0; i < 3; i++)
            _businesses.Add(new()
            {
                Id = i.ToString(),
                Name = "business #" + i,
                Rating = i,
                Price = "$" + (i + 1) + "0.00",
                Phone = "(555) 555 - 5555",
                Location = new Location() { City = "Testing", State = "#" + i}
            });  
    }

    /// <summary>
    /// Creates a single business object
    /// </summary>
    private void CreateReviews()
    {
        _reviews = new List<Review>();
        
        for (var i = 0; i < 3; i++)
            _reviews.Add(new()
            {
                Id = i.ToString(),
                Name = "business #" + i,
                Rating = i,
                TimeCreated = DateTime.Now,
                User = new() { Id = i.ToString()  }
            });  
    }

    private YelpService CreateYelpService()
    {
        var service = new YelpService(_mockLogger, _mockHttpClientFactory.Object, _mockConfiguration, _mockYelpOptions.Object);
        return service;
    }

    [Test]
    public void CreateClient_StateUnderTest_Valid()
    {
        CreateMockHttpClient();
        var service = CreateYelpService();
        var result = service.CreateClient();

        Assert.That(result, Is.InstanceOf<HttpClient>());
    }

    [Test]
    public void GetBusinessById_Valid()
    {
        CreateMockHttpClient();

        var service = CreateYelpService();
        var response = service.GetBusinessById("0");
        
        string id = response.Result.Data.Id;

        Assert.That(id, Is.EqualTo("0"));
        Assert.That(response.Result.IsSuccess.Equals(true));
    }
                    
    [Test]
    public void GetBusinessByPhone_Valid()
    {
        var yelpResponse = CreateYelpResponse();
        CreateMockHttpClient(yelpResponse);
        
        var service = CreateYelpService();
        var response = service.GetBusinessByPhone("(555) 555 - 5555");

        Assert.That(response.Result.Data.Phone, Is.EqualTo("(555) 555 - 5555"));
        Assert.That(response.Result.IsSuccess.Equals(true));
    }

    [Test]
    public void GetBusinessesByLocation_Valid()
    {
        var yelpResponse = CreateYelpResponse();
        CreateMockHttpClient(yelpResponse);

        var service = CreateYelpService();
        var response = service.GetBusinessesByLocation("Some Location");
        var lastBusiness = response.Result.Data.Last();

        Assert.That(response.Result.IsSuccess.Equals(true));
        Assert.That(response.Result.Data.Count, Is.EqualTo(3));
        Assert.That(lastBusiness.Location.City + " " + lastBusiness.Location.State, Is.EqualTo("Testing #2" ));
    }

    [Test]
    public void GetBusinessesByKeywords_Valid()
    {
        var yelpResponse = CreateYelpResponse();
        CreateMockHttpClient(yelpResponse);

        var service = CreateYelpService();
        var searchDto = new SearchDto() 
        {
            Location = "Test Location",
            Lat = "0",
            Long = "0",
            Terms = ["test", "term"],        
        };
        var response = service.GetBusinessesByKeywords(searchDto);
        var lastBusiness = response.Result.Data.Last();
        
        Assert.That(response.Result.IsSuccess.Equals(true));
        Assert.That(response.Result.Data.Count, Is.EqualTo(3));
        Assert.That(lastBusiness.Location.City + " " + lastBusiness.Location.State, Is.EqualTo("Testing #2" ));
    }

    [Test]
    public void GetReviewsById_Valid()
    {
        var yelpResponse = CreateYelpResponse();
        CreateMockHttpClient(yelpResponse);

        var service = CreateYelpService();

        var response = service.GetReviewsById("2");
        var lastReview = response.Result.Data.Last();

        Assert.That(response.Result.IsSuccess.Equals(true));
        Assert.That(response.Result.Data, Has.Count.EqualTo(3));
        Assert.That(lastReview.User.Id, Is.EqualTo("2"));
    }
}
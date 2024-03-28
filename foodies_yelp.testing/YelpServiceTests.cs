using foodies_yelp.Services;
using NUnit.Framework;
using Moq;
using foodies_yelp.Models.Dtos.Requests;
using foodies_yelp.Models.Dtos.Responses.Yelp;
using foodies_yelp.Models.Dtos.Responses;

namespace foodies_yelp.testing;

public class YelpServiceTests
{
    private List<Business> _businesses = new List<Business>(3);

    [SetUp]
    public void Setup()
    {
        var service = new Mock<YelpService>();

        // service.Setup(x => x.GetBusinesses(It.IsAny<SearchDto>()))
        //     .ReturnsAsync(new APIResult<List<Business>>() 
        //     { IsSuccess = true, Data = });
        service.Setup(x => x.GetBusinessByPhone(It.IsAny<string>()));
    }

    private static List<Business> CreateBusinesses()
    {
        foreach(var business in _businesses) 
            _businesses.Add()

        return new List<Business>();
    }

    [Test]
    public void GetBusinessById()
    {
        
        Assert.Pass();
    }
}
using foodies_yelp.Services;
using NUnit.Framework;
using Moq;

namespace foodies_yelp.testing;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        var app = new Mock<WebApplication>();
        var YelpService = app.Setup(x => x.Services.GetRequiredService<YelpService>());
    }

    [Test]
    public void GetBusinessById()
    {
        
        Assert.Pass();
    }
}
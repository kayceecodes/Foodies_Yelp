using Moq;
using Moq.Protected;

namespace foodies_yelp.tests;

public static class Assertions
{
    public static void Verify(this Mock<HttpMessageHandler> mock, Func<HttpRequestMessage, bool> match)
    {
        mock.Protected().Verify(
            "GetAsync",
            Times.Exactly(1), // we expected a single external request
            ItExpr.Is<HttpRequestMessage>(req => match(req)
            ),
            ItExpr.IsAny<CancellationToken>()
        );
    }

}

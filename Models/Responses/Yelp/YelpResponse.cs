using foodies_yelp.Models.Dtos;

namespace foodies_yelp.Models.Responses.Yelp;

public class YelpResponse
{
    public List<Business>? Businesses { get; set; }
    public int Total { get; set; }
    public Region? Region { get; set; }
    public List<Review> Reviews { get; set; }
}

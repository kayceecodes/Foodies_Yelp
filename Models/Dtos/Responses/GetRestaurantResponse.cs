using foodies_yelp.Models.Dtos.Responses.Yelp;

namespace foodies_yelp;

public class GetRestaurantResponse
{
    public string Alias { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string URL { get; set; } = string.Empty;
    public int ReviewCount { get; set; }
    public List<Review> Reviews { get; set; }
    public List<Category> Categories { get; set; }
    public int Rating { get; set; }
    public Coordinates Coordinates { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Price { get; set; } = string.Empty;
}

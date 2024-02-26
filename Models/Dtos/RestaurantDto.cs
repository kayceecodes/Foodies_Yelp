using foodies_yelp.Models;

namespace foodies_yelp.Models.Dtos;

public class RestaurantDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<Branch> Branches { get; set; } = new List<Branch>();
    public int Rating { get; set; }
}

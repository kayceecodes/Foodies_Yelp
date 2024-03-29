using foodies_yelp.Models.Dtos.Responses.Yelp;

namespace foodies_yelp;

public class GetReviewResponse
{
    public string Id { get; set; }
    public string Text { get; set; }
    public string Url { get; set; }
    public int Rating { get; set; }
    public DateTime TimeCreated { get; set; }
    public string UsersName { get; set; }
    public string ImageUrl { get; set; }
}


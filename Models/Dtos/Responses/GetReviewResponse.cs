using foodies_yelp.Models.Dtos.Responses.Yelp;

namespace foodies_yelp;

public class GetReviewResponse
{
    public string Text { get; set; }
    public int Rating { get; set; }
    public DateTime TimeCreated { get; set; }
    public string UsersName { get; set; }
}


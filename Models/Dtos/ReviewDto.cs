namespace foodies_yelp.Models.Dtos;

public class ReviewDto
{
    public string Id { get; set; }
    public string Url { get; set; }
    public string Text { get; set; }
    public int Rating { get; set; }
    public UserDto User { get; set; }
    public DateTime TimeCreated { get; set; }
}

public class UserDto 
{
    public string Id { get; set; }
    public string ProfileUrl { get; set; }
    public string ImageUrl { get; set; }
    public string Name { get; set; }
}
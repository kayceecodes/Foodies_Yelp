namespace foodies_yelp.Models.Dtos.Responses.Yelp;

public class Review : User
{
    public string Id { get; set; }
    public string Url { get; set; }
    public string Text { get; set; }
    public int Rating { get; set; }
    public DateTime TimeCreated { get; set; }
    public User User { get; set; }
}

public class User 
{
    public string Id { get; set; }
    public string ProfileUrl { get; set; }
    public string ImageUrl { get; set; }
    public string Name { get; set; }
}
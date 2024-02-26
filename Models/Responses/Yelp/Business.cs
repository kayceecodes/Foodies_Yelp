namespace foodies_yelp.Models.Responses.Yelp;

public class Business
{
    public string Id { get; set; }
    public string? Alias { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Image_url { get; set; }
    public bool Is_closed { get; set; }
    public string? Url { get; set; }
    public int Review_count { get; set; }
    public List<Category> Categories { get; set; } = null!;
    public double Rating { get; set; }
    public Coordinates? Coordinates { get; set; }
    public List<string>? Transactions { get; set; }
    public string? Price { get; set; }
    public Location? Location { get; set; } = null!;
    public string? Phone { get; set; } = null!;
    public string? Display_phone { get; set; } = null!;
    public double Distance { get; set; }
}

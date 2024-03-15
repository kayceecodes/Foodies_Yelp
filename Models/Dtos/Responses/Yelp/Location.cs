namespace foodies_yelp.Models.Dtos.Responses;

public class Location
{
    public string Address1 { get; set; } = null!;
    public string? Address2 { get; set; }
    public string? Address3 { get; set; }
    public string City { get; set; } = null!;
    public string Zip_code { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string? State { get; set; }
    public List<string> Display_address { get; set; } = null!;
}

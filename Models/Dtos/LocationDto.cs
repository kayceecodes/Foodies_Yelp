namespace foodies_yelp.Models.Dtos;

public class LocationDto
{
    public List<string> Places { get; set; }
    public Decimal Long { get; set; }
    public Decimal Lat { get; set; }
    public int Limit { get; set; }
}

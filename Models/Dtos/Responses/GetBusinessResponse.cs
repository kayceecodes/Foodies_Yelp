using foodies_yelp.Models.Dtos.Responses.Yelp;

namespace foodies_yelp;

public class GetBusinessResponse

{
    public string Id { get; set; }
    public string Alias { get; set; }
    public string Name { get; set; }
    public string URL { get; set; }
    public int Rating { get; set; }
    public int ReviewCount { get; set; }
    public List<string> Categories { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Price { get; set; }
}

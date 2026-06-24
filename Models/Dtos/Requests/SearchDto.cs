using System.ComponentModel.DataAnnotations;

namespace foodies_yelp.Models.Dtos.Requests;

public class SearchDto
{

    [Required, Range(0, 90)]
    public double Latitude { get; set; }

    [Required, Range(0, 180)]
    public double Longitude { get; set; }
    
    [Range(0, 8047)] // (Rounded up) (5 miles == 8046.72)
    public double? RadiusMeters { get; set; } // Yelp Fusion API has its own default range, it's business-population based. Yelp Fusion Api takes an Integer

    public List<string>? Keywords { get; set; } 

    public int Offset { get; set; }

    public int Limit { get; set; }
}

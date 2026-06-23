using System.ComponentModel.DataAnnotations;

namespace foodies_yelp.Models.Dtos.Requests;

public class SearchDto
{
    [Required]
    public string? Category { get; set; }

    [Required]
    public double Lat { get; set; }

    [Required]
    public double Long { get; set; }
    
    [Range(0, 8047)]
    public int? RadiusMeters { get; set; } // Yelp API has its own default range, it's business-population based

    [Required]
    public List<string>? Keywords { get; set; } 

    public int Limit { get; set; }
}

  //var query = $"?sort_by=best_match&limit={dto.Limit}&term={terms}&location={dto.Location}&latitude={dto.Lat}&longitude={dto.Long}";

public class SearchRequest 
{
    public List<string> Keywords { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string Offset { get; set; }
    public string Limit { get; set; }
    public double? RadiusMeters { get; set; }
}    

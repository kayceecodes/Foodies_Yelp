namespace foodies_yelp.Models.Options;

public class YelpOptions
{
    public string ApiKey { get; set; } // Dev Api Key, after section name is referenced
    public string ApiKeyName { get; set; } // Production Api Key Name
    public string BaseAddress { get; set; }
}

namespace foodies_yelp.Models;

    public class SearchTerm
    {
        public string Term { get; set; }
        public string Location { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Radius { get; set; }
        public string Categories { get; set; }    
    }

using System.ComponentModel.DataAnnotations;
using foodies_yelp;

namespace foodies_yelp.Models;

public class Restaurant
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<Branch> Branches { get; set; } = new List<Branch>();
    public int Rating { get; set; }
}

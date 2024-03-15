using foodies_yelp.Models.Dtos.Requests;
using foodies_yelp.Models.Dtos.Responses;
using foodies_yelp.Models.Dtos.Responses.Yelp;

namespace foodies_yelp;


interface IYelpService
{
    Task<APIResult<Business>> GetBusinessById(string id);
    Task<APIResult<List<Business>>> GetBusinessesByLocation(string location);
    Task<APIResult<Business>> GetBusinessByPhone(string phonenumber);
    Task<APIResult<List<Business>>> GetBusinesses(SearchDto dto);
    Task<APIResult<List<Review>>> GetReviewsById(string id);
    HttpClient CreateClient();
}
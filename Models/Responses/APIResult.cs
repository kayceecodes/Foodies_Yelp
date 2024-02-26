using System.Net;

namespace foodies_yelp.Models.Responses;

public class APIResult<T>
{
    public APIResult()
    {
        ErrorMessages = new List<string>();
    }
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public List<string> ErrorMessages { get; set; }
}

public class APIResult
{
    public APIResult()
    {
        ErrorMessages = new List<string>();
    }
    public bool IsSuccess { get; set; }
    public object Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public List<string> ErrorMessages { get; set; }
}
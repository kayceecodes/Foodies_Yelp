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
    public List<string>? ErrorMessages { get; set; }
    public Exception? Exception { get; set; }

    public static APIResult<T> Fail(string message, HttpStatusCode statusCode, Exception? exception)
        {
            return new APIResult<T>
            {
                IsSuccess = false,
                Data = default,
                StatusCode = statusCode,
                ErrorMessages = new() {message},
                Exception = exception,
            };
        }

        public static APIResult<T> Fail(string message, HttpStatusCode statusCode)
        {
            return new APIResult<T>
            {
                IsSuccess = false,
                Data = default,
                StatusCode = statusCode,
                ErrorMessages = new() {message},
            };
        }

        public static APIResult<T> Pass(T data)
        {
            return new APIResult<T>
            {
                IsSuccess = true,
                Data = data,
                StatusCode = HttpStatusCode.OK,
                ErrorMessages = null,
                Exception = default,
            };
        }
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
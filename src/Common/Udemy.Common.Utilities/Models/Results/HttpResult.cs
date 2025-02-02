namespace Udemy.Common.Utilities.Models.Results;

public record HttpResult<T> : Result<T>, IHttpResult<T>
{
    public int StatusCode { get; init; }

    public static HttpResult<T?> Success(T? data, int statusCode, string message = "") => new()
    {
        IsSuccess = true,
        Message = message,
        Error = Error.None,
        Data = data,
        StatusCode = statusCode
    };

    public static new HttpResult<T?> Failure(Error error) => new()
    {
        IsSuccess = false,
        Message = error.Message,
        Error = error
    };
}

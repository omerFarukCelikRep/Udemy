namespace Udemy.Common.Utilities.Models.Results;
public static class ResultExtensions
{
    public static HttpResult<T?> ToHttpResult<T>(this Result<T?> result, int statusCode) => new()
    {
        IsSuccess = result.IsSuccess,
        Message = result.Message,
        Error = result.Error,
        Data = result.Data,
        StatusCode = statusCode
    };
}

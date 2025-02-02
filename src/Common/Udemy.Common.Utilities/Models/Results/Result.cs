namespace Udemy.Common.Utilities.Models.Results;
public record Result : IResult
{
    public bool IsSuccess { get; init; }

    public required string Message { get; init; }

    public Error? Error { get; init; }

    public static Result Success(string message = "") => new()
    {
        IsSuccess = true,
        Message = message,
        Error = Error.None
    };

    public static Result Failure(Error error) => new()
    {
        IsSuccess = false,
        Message = error.Message,
        Error = error
    };
}

public record Result<T> : Result, IResult<T>
{
    public T? Data { get; init; }

    public static Result<T?> Success(T? data, string message = "") => new()
    {
        IsSuccess = true,
        Message = message,
        Error = Error.None,
        Data = data
    };

    public static new Result<T?> Failure(Error error) => new()
    {
        IsSuccess = false,
        Message = error.Message,
        Error = error
    };
}

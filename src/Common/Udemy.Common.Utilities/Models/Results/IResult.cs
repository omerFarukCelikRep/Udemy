namespace Udemy.Common.Utilities.Models.Results;
public interface IResult
{
    bool IsSuccess { get; }
    string? Message { get; }
    Error? Error { get; }
}

public interface IResult<out T> : IResult
{
    T? Data { get; }
}

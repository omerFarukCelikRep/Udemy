namespace Udemy.Common.Utilities.Models.Results;

public interface IHttpResult<out T> : IResult<T>
{
    int StatusCode { get; }
}

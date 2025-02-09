using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Udemy.Common.Logging.Behaviors;
public class RequestResponseLoggingBehavior<TRequest, TResponse>(ILogger<RequestResponseLoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var correlationId = Guid.CreateVersion7();
        string requestJson = JsonSerializer.Serialize(request);

        logger.LogInformation("Handling request {CorrelationID}: {Request}", correlationId, requestJson);

        TResponse? response = await next();
        string responseJson = JsonSerializer.Serialize(response);

        logger.LogInformation("Response for {Correlation}: {Response}", correlationId, responseJson);

        return response;
    }
}

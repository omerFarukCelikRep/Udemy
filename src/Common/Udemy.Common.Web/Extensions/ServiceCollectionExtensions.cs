using Microsoft.Extensions.DependencyInjection;
using Udemy.Common.Web.Handlers;

namespace Udemy.Common.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }
}

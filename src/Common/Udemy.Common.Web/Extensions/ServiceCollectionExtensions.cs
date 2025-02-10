using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Udemy.Common.Web.Handlers;
using Udemy.Common.Web.Options;

namespace Udemy.Common.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    public static IServiceCollection AddCors(this IServiceCollection services, IReadOnlyList<string>? policyNames = default(List<string>))
    {
        services.AddOptions<CorsOptions>()
               .BindConfiguration(CorsOptions.SectionName);

        ServiceProvider serviceProvider = services.BuildServiceProvider();
        CorsOptions corsOptions = serviceProvider.GetRequiredService<IOptions<CorsOptions>>().Value;

        services.AddCors(options =>
        {
            if (policyNames is not { Count: > 0 })
                options.AddPolicy(CorsPolicyOptions.DefaultSectionName, builder => ConfigurePolicy(builder, CorsPolicyOptions.DefaultSectionName));

            foreach (string policyName in policyNames!)
            {
                options.AddPolicy(policyName, builder => ConfigurePolicy(builder, policyName));
            }

            options.DefaultPolicyName = CorsPolicyOptions.DefaultSectionName;
        });

        return services;

        void ConfigurePolicy(Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder builder, string policyName)
        {
            CorsPolicyOptions policyOptions = corsOptions.Policies[policyName];
            builder.WithOrigins(policyOptions.Origins);
            builder.WithHeaders(policyOptions.Headers);
            builder.WithMethods(policyOptions.Methods);
        }
    }
}

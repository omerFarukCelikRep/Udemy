using Microsoft.AspNetCore.Builder;

namespace Udemy.Common.Web.Extensions;
public static class ApplicationBuilderExtensions
{
    public static void UseGlobalExceptionHandler(this IApplicationBuilder applicationBuilder) => applicationBuilder.UseExceptionHandler();
}

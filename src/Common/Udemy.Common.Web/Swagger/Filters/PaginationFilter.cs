using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Udemy.Common.Models.Constants;

namespace Udemy.Common.Web.Swagger.Filters;
public class PaginationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context) => operation.Parameters =
        [
            new OpenApiParameter
            {
                Name = Header.XPage,
                In = ParameterLocation.Header,
                Description = "Pagination Page Index Header",
                Required = false
            },
            new OpenApiParameter
            {
                Name = Header.XPageSize,
                In = ParameterLocation.Header,
                Description = "Pagination Page Size Header",
                Required = false
            }
        ];
}

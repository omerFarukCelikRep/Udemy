using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Udemy.Common.Authentication.Helpers;
using Udemy.Common.Models.Constants;

namespace Udemy.Common.Authentication.Principals;
public class DomainPrincipal(IHttpContextAccessor contextAccessor)
    : IDomainPrincipal
{
    public string? Id => GetClaim<string>(ClaimTypes.NameIdentifier);
    public string? FirstName => GetClaim<string>(ClaimTypes.GivenName);
    public string? LastName => GetClaim<string>(ClaimTypes.Surname);

    private T? GetClaim<T>(string claimName)
    {
        string? token = contextAccessor?.HttpContext?.Request?.Headers[Header.Authorization]
                                                   .FirstOrDefault()?.Split(" ")
                                                   .LastOrDefault();
        Dictionary<string, string> claims = [];
        if (!string.IsNullOrWhiteSpace(token))
            claims = JwtHelper.DecodeJwt(token).Claims;

        return claims.Where(x => x.Key == claimName)
                     .Select(x => x.Value)
                     .Cast<T>()
                     .FirstOrDefault();
    }
}

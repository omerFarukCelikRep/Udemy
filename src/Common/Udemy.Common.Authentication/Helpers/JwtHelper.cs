using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Udemy.Common.Authentication.Models;

namespace Udemy.Common.Authentication.Helpers;
public static class JwtHelper
{
    private static readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

    private static DecodedToken DecodeJwt(JwtSecurityToken token)
    {
        string keyId = token.Header.Kid;
        var audience = token.Audiences.ToList();
        var claims = token.Claims.Select(claim => (claim.Type, claim.Value))
                                 .ToDictionary();
        return new DecodedToken(keyId,
                                token.Issuer,
                                audience,
                                claims,
                                token.ValidTo,
                                token.SignatureAlgorithm,
                                token.RawData,
                                token.Subject,
                                token.ValidFrom,
                                token.EncodedHeader,
                                token.EncodedPayload);

    }
    public static JwtSecurityToken ConvertJwtStringToJwtSecurityToken(string? jwt)
    {
        JwtSecurityToken token = _jwtSecurityTokenHandler.ReadJwtToken(jwt);

        return token;
    }

    public static string GetUser(string token)
    {
        JwtSecurityToken jwtToken = ConvertJwtStringToJwtSecurityToken(token);
        DecodedToken decodedToken = DecodeJwt(jwtToken);

        return decodedToken.Claims.FirstOrDefault(x => x.Key == ClaimTypes.NameIdentifier).Value;
    }
}

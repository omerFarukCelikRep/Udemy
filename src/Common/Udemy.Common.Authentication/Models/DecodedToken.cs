namespace Udemy.Common.Authentication.Models;
public record DecodedToken(string KeyId,
                           string Issuer,
                           IReadOnlyCollection<string> Audience,
                           Dictionary<string, string> Claims,
                           DateTime ValidTo,
                           string SignatureAlgorithm,
                           string RawData,
                           string Subject,
                           DateTime ValidFrom,
                           string EncodedHeader,
                           string EncodedPayload);

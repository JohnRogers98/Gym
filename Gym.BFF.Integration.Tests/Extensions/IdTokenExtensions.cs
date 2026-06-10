using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

namespace Gym.OAuth.Extensions;

public static class IdTokenExtensions
{
    public static String Sign(this IdToken idToken, SigningCredentials signingCredentials)
    {
        var claimsIdentity = new ClaimsIdentity(new[]
       {
                new Claim(JwtRegisteredClaimNames.Iss, idToken.Issuer),
                new Claim(JwtRegisteredClaimNames.Sub, idToken.Subject),
                new Claim(JwtRegisteredClaimNames.Aud, idToken.Audience),
                new Claim(JwtRegisteredClaimNames.Exp, idToken.Expiration.ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iat, idToken.IssuedAt.ToString(), ClaimValueTypes.Integer64)
            });

        if (idToken.AuthenticationTime.HasValue)
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.AuthTime, idToken.AuthenticationTime.Value.ToString(), ClaimValueTypes.Integer64));
        if (idToken.Nonce is not null)
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Nonce, idToken.Nonce));
        if (idToken.AuthenticationContextClassReference is not null)
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Acr, idToken.AuthenticationContextClassReference));
        if (idToken.AuthenticationMethodsReferences is not null)
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Amr, JsonSerializer.Serialize(idToken.AuthenticationMethodsReferences), JsonClaimValueTypes.JsonArray));
        if (idToken.AtHash is not null)
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.AtHash, idToken.AtHash));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            SigningCredentials = signingCredentials,
            TokenType = "id_token+jwt",

        };

        return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
    }
}

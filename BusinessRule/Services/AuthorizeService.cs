using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using AutoWrapper.Exceptions;
using DataAccess.RequestModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BusinessRule.Services;

public class AuthorizeService : IAuthorizeService
{
    public async Task<string> AuthorizeLoginService(AuthorizeDto request)
    {
        string result =  ValidateToken(request.requestToken);
        return result;
    }
    
    public string ValidateToken(string token)
    {
        if (token == null) 
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("/39igoRLNni66jm4SzCjfP6Qdfji/CL4I0u2qNsmniQ=");
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = "AlirezaBanking",
                ValidAudience = "AlirezaBanking",
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            string userId = jwtToken.Claims.First(x => x.Type == "id").Value;

            // return user id from JWT token if validation successful
            return userId;
        }
        catch
        {
            throw new ApiProblemDetailsException(new ProblemDetails()
            {
                Status = (int?)HttpStatusCode.Unauthorized,
                Title = "Authorization Failed.",
            });
        }
    }
}
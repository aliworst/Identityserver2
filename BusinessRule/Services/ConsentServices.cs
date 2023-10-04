using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Text;
using AutoWrapper.Exceptions;
using DataAccess.RequestModel;
using DataAccess.ResponseModel;
using EntityFramework.DbModels;
using EntityFramework.Enumrations;
using EntityFramework.Repositories;
using HelpMate.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SystemClock = Microsoft.Extensions.Internal.SystemClock;

namespace BusinessRule.Services;

public class ConsentServices : IConsentServices
{
    protected readonly IConsentRepository ConsentRepository;

    public ConsentServices(IConsentRepository consentRepository)
    {
        ConsentRepository = consentRepository;
    }

    public async Task<CreateConsentResponse> CreateConsentAsync(ConsentDto request)
    {
        string Parameters = $"{{MaximumDailyTransferAmount:{request.MaximumDailyTransferAmount}" +
                            $"  MaximumDailyTransactionCount:{request.MaximumDailyTransactionCount}" +
                            $"  MaximumAmountPerTransfer:{request.MaximumAmountPerTransfer}" +
                            $"  CreditorAccounts:{String.Join(",", request.CreditorAccounts)}}}";
        var dbConsent = new Consent()
        {
            Id = Guid.NewGuid().ToString(),
            ClientId = request.ClientId,
            Scope = String.Join(",", request.Scopes),
            Expiration = request.ExpirationDate,
            CreationTime = new SystemClock().UtcNow,
            Type = "vrp",
            Parameters = Parameters,
            Status = Status.AwaitingAuthorization.ToString()
        };
        var insertedConsent = await ConsentRepository.CreateConsentAsync(dbConsent);
        string token = GenerateToken(dbConsent);
        var response = new CreateConsentResponse()
        {
            ConsentId = dbConsent.Id,
            Request = token,
        };
        return response;
    }

    public string GenerateToken(Consent consent)
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(
            "/39igoRLNni66jm4SzCjfP6Qdfji/CL4I0u2qNsmniQ=");
        if (consent != null)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", consent.Id), new Claim("clientId", consent.ClientId),
                    new Claim("Scope", consent.Scope), new Claim("Expiration", consent.Expiration.ToString()),
                    new Claim("CreationTime", consent.CreationTime.ToString()), new Claim("Type", consent.Type),
                    new Claim("Parameters", consent.Parameters), new Claim("Status", consent.Status),
                    new Claim("iss", "AlirezaBanking"),new Claim("aud", "AlirezaBanking")
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        else 
            throw new ApiProblemDetailsException(new ProblemDetails()
            {
                Status = (int?)HttpStatusCode.BadRequest,
                Title = "A problem when generating token",
            });
        
    }
}


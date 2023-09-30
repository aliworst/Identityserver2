using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccess.RequestModel;
using DataAccess.ResponseModel;
using EntityFramework.DbModels;
using EntityFramework.Enumrations;
using EntityFramework.Repositories;
using Microsoft.AspNetCore.Authentication;
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
        var dbConsent = new Consent()
        {
            Id = Guid.NewGuid().ToString(),
            ClientId = request.ClientId,
            Scope = request.Scopes.ToString(),
            Expiration = request.ExpirationDate,
            CreationTime = new SystemClock().UtcNow,
            Type = new List<string>
            {
                "vrp"
            }.ToString(),
            Parameters = request.MaximumDailyTransferAmount + 
                         request.MaximumDailyTransactionCount +
                         request.MaximumAmountPerTransfer  +
                         request.CreditorAccounts.ToString(),
            // Parameters = new Parameters
            // {
            //     MaximumDailyTransferAmount = request.MaximumDailyTransferAmount,
            //     MaximumDailyTransactionCount = request.MaximumDailyTransactionCount,
            //     MaximumAmountPerTransfer = request.MaximumAmountPerTransfer,
            //     Creditors = request.CreditorAccounts
            // },
            Status = Status.AwaitingAuthorization
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
    var key = Encoding.ASCII.GetBytes("-----BEGIN OPENSSH PRIVATE KEY-----\nb3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAABFwAAAAdzc2gtcn\nNhAAAAAwEAAQAAAQEAx5Z4ZlnHkPC63o6qtj8hsQl5B/18S7WswxM56q99puaYjl4cbd4L\n0f4h36vVwArY6RaYoWIHAQQwbZsDMDtiZU0j0aKQKV+3OTvNJ2JpqY0cala35JNPeI7Es0\nqFfSiLp2d2ZvMfV+nJpJqGzE5ckgRh9eqJO9gqbBhaWprGrX61gF9c55h6HnR4mC462Wqg\n/vIlnXkWTi/jZTe9gcNu1kQOgtAToJZr4eFnHnGleIg6XA+8wF2KI3usYYTsKH4tiOa3EE\n2FQLx0c70c6626XzpLTo9nxbLL15GQbU2Nc0YjHULQ9cvlDwdz6EZsZIWI+B0Bjp9BKtGc\nL/aK/+mqGQAAA8gupOSvLqTkrwAAAAdzc2gtcnNhAAABAQDHlnhmWceQ8Lrejqq2PyGxCX\nkH/XxLtazDEznqr32m5piOXhxt3gvR/iHfq9XACtjpFpihYgcBBDBtmwMwO2JlTSPRopAp\nX7c5O80nYmmpjRxqVrfkk094jsSzSoV9KIunZ3Zm8x9X6cmkmobMTlySBGH16ok72CpsGF\npamsatfrWAX1znmHoedHiYLjrZaqD+8iWdeRZOL+NlN72Bw27WRA6C0BOglmvh4WcecaV4\niDpcD7zAXYoje6xhhOwofi2I5rcQTYVAvHRzvRzrrbpfOktOj2fFssvXkZBtTY1zRiMdQt\nD1y+UPB3PoRmxkhYj4HQGOn0Eq0Zwv9or/6aoZAAAAAwEAAQAAAQAEuXZxex/oOb/n/hnj\nD9eCU1EUHqyki3u4Gmpn4xjnAdUL+a+pQh+m1uHXuOo/Di1YRyVRx0+XIebrkUcgCyfRx2\nOuVgTHlfEbtQ4EJ/uMUeohnO/kGetAVVDUAFLsmtsYxAWKJlMlnO5zAQgT1Kmy+HuWe04d\npoj1NMEJcq8lSCx8b5bAj+byULuGxfODtokVaHHnqvmNVQZp6EHd8NIOhtL/0y50K7u6OD\noFMqxA0Xi1+JxUTq4N2yeLgbrXthhV0xqCJ85JFTF2DH8tnKONMfQ74jFNxHu5txtheuzL\niqX7/Df532GXnKPeuq+mx96CnqVP9JD2Cbd0C/s0kTPJAAAAgQDclz8rkkTu2PE9MFsCXx\nQGv+/CDiHBDVEHZLIEcgAKokFiwO18Gd9WzwCro5PrWOuVm3F9zAOPzBUNHsr2hPiF9ADZ\nCjyq+ho6imomAUvKZ2d+Q3XuyYdqT50Y6/YB0cYrdh+5ITh+IZNsnStvMMtWNRz/XTMAMj\nSbUyxILRGfCAAAAIEA9pTVQBTHrRtSvrFt7QoGheJajpCXjl7q/Au4Fn6RHwuiwmhl1G2T\n4WsKRejsC2F89T4K8e6RmZyEtefhWX0+mCpBxBVgZgwVUuIfa9KMdqfxpgqyqLX+3hYCfg\n+H3GeB4h6enNLYmGHCO2DoynPCDhNAozUGjIM5FPTXtyMe8R8AAACBAM82Hanx84k1Wb+0\nHxZrRWQPP5VFgKDbXyDz2JR/ZcxS5pLYKE2bme92vOg1E9RCw7qz998FOeSOr6nMDFuI4S\n9KfpwVDraL+qazERzkku+VXxKI4UfsiKrV4kfYfwSUKhgTSlRa8WhYodPAQroC4oK79xzn\nNACvEyqjna+T5WXHAAAAEnJvb3RAc3J2NzQ2NzgzNTYzMg==\n-----END OPENSSH PRIVATE KEY-----\n");
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[] { new Claim("id", consent.Id) }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}
}


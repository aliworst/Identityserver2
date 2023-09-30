using DataAccess.RequestModel;
using DataAccess.ResponseModel;
using Duende.IdentityServer.Models;

namespace BusinessRule.Services;

public interface IConsentServices
{
    Task<CreateConsentResponse> CreateConsentAsync(ConsentDto request);
}
using EntityFramework.DbModels;

namespace EntityFramework.Repositories;

public interface IConsentRepository
{
    Task<int> CreateConsentAsync(Consent dbConsent);
}
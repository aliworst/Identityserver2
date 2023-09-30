using EntityFramework.DbContexts;
using EntityFramework.DbModels;

namespace EntityFramework.Repositories;

public class ConsentRepository : IConsentRepository
{
    protected readonly ConsentDbContext ConsentDbContext;
    
    public ConsentRepository(ConsentDbContext consentDbContext)
    {
        ConsentDbContext = consentDbContext;
    }
    public async Task<int> CreateConsentAsync(Consent dbConsent)
    {
        var consentResult = await ConsentDbContext.Consent.AddAsync(dbConsent);
        return await ConsentDbContext.SaveChangesAsync();
    }
}
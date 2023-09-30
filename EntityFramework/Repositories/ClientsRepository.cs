using Duende.IdentityServer.EntityFramework.Entities;
using EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Repositories;

public class ClientsRepository : IClientsRepository
{
    protected readonly AdminDbContexts AdminDbContext;
    
    public ClientsRepository(AdminDbContexts iAdminDbContext)
    {
        AdminDbContext = iAdminDbContext;
    }

    public virtual Task<Client?> GetClientAsync(int clientId)
    {
        return AdminDbContext.Clients
            .Include(x => x.AllowedGrantTypes)
            .Include(x => x.RedirectUris)
            .Include(x => x.PostLogoutRedirectUris)
            .Include(x => x.AllowedScopes)
            .Include(x => x.ClientSecrets)
            .Include(x => x.Claims)
            .Include(x => x.IdentityProviderRestrictions)
            .Include(x => x.AllowedCorsOrigins)
            .Include(x => x.Properties)
            .Where(x => x.Id == clientId)
            .AsNoTracking()
            .SingleOrDefaultAsync();
    }

    public virtual async Task<int> UpdateClientsAsync(Client client,
        bool updateClientClaims,
        bool updateClientProperties)
    {
        await RemoveClientRelationsAsync(client, updateClientClaims, updateClientProperties);
        
        AdminDbContext.Clients.Update(client);
        
        return await AdminDbContext.SaveChangesAsync();
    }

    public Task<int> AddClientAsync(Client client)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<int> RemoveClientAsync(Client client)
    {
        AdminDbContext.Clients.Remove(client);

        return await AdminDbContext.SaveChangesAsync();
    }
    
    private async Task RemoveClientRelationsAsync(Client client, bool updateClientClaims,
        bool updateClientProperties)
    {
        //Remove old allowed scopes
        var clientScopes = await AdminDbContext.ClientScopes.Where(x => x.Client.Id == client.Id).ToListAsync();
        AdminDbContext.ClientScopes.RemoveRange(clientScopes);

        //Remove old grant types
        var clientGrantTypes = await AdminDbContext.ClientGrantTypes.Where(x => x.Client.Id == client.Id).ToListAsync();
        AdminDbContext.ClientGrantTypes.RemoveRange(clientGrantTypes);

        //Remove old redirect uri
        var clientRedirectUris = await AdminDbContext.ClientRedirectUris.Where(x => x.Client.Id == client.Id).ToListAsync();
        AdminDbContext.ClientRedirectUris.RemoveRange(clientRedirectUris);

        //Remove old client cors
        var clientCorsOrigins = await AdminDbContext.ClientCorsOrigins.Where(x => x.Client.Id == client.Id).ToListAsync();
        AdminDbContext.ClientCorsOrigins.RemoveRange(clientCorsOrigins);

        //Remove old client id restrictions
        var clientIdPRestrictions =
            await AdminDbContext.ClientIdPRestrictions.Where(x => x.Client.Id == client.Id).ToListAsync();
        AdminDbContext.ClientIdPRestrictions.RemoveRange(clientIdPRestrictions);

        //Remove old client post logout redirect
        var clientPostLogoutRedirectUris =
            await AdminDbContext.ClientPostLogoutRedirectUris.Where(x => x.Client.Id == client.Id).ToListAsync();
        AdminDbContext.ClientPostLogoutRedirectUris.RemoveRange(clientPostLogoutRedirectUris);

        //Remove old client claims
        if (updateClientClaims)
        {
            var clientClaims = await AdminDbContext.ClientClaims.Where(x => x.Client.Id == client.Id).ToListAsync();
            AdminDbContext.ClientClaims.RemoveRange(clientClaims);
        }

        //Remove old client properties
        if (updateClientProperties)
        {
            var clientProperties = await AdminDbContext.ClientProperties.Where(x => x.Client.Id == client.Id).ToListAsync();
            AdminDbContext.ClientProperties.RemoveRange(clientProperties);
        }
    }
}
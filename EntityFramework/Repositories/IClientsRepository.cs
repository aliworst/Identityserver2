using Duende.IdentityServer.EntityFramework.Entities;

namespace EntityFramework.Repositories;

public interface IClientsRepository 
{
    Task<int> AddClientAsync(Client client);
    Task<int> RemoveClientAsync(Client client);
    Task<Client?> GetClientAsync(int clientId);
    Task<int> UpdateClientsAsync(Client client, bool updateClientClaims,
        bool updateClientProperties);
}
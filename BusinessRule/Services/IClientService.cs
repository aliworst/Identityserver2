using Duende.IdentityServer.EntityFramework.Entities;

namespace BusinessRule.Services;

public interface IClientService
{
    void CreateClient(Client client);

    Task<Client?> GetClientAsync(int clientId);

    Task<int> UpdateClientsAsync(Client data, bool updateClientClaims = false,
        bool updateClientProperties = false);

    Task<int> RemoveClientAsync(Client client);
}
using Duende.IdentityServer.EntityFramework.Entities;
using EntityFramework.Repositories;

namespace BusinessRule.Services;

public class ClientService : IClientService
{
    protected readonly IClientsRepository ClientsRepository;

    public ClientService(IClientsRepository clientsRepository)
    {
        ClientsRepository = clientsRepository;
    }

    public void CreateClient(Client client)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<Client?> GetClientAsync(int clientId)
    {
        var client = await ClientsRepository.GetClientAsync(clientId);

        var clientDto = client;

        return clientDto;
    }

    public async Task<int> UpdateClientsAsync(Client client, bool updateClientClaims = false, bool updateClientProperties = false)
    {
        var updated = await ClientsRepository.UpdateClientsAsync(client, true, true);

        return updated;
    }

    public virtual async Task<int> RemoveClientAsync(Client client)
    {
        var deleted = await ClientsRepository.RemoveClientAsync(client);

        return deleted;
    }
}
using System.Net;
using AutoWrapper.Exceptions;
using BusinessRule.Services;
using DataAccess.RequestModel;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Identityserver.Controller;
[ApiController]
[Route("Api/V1/Clients")]
public class IdentityServerController : ControllerBase
{
    private readonly IConfigurationDbContext _configurationDbContext;
    private readonly IClientService _clientsServices;
    public IdentityServerController(IConfigurationDbContext configurationDbContext, IClientService clientsServices)
    {
        _clientsServices = clientsServices;
        _configurationDbContext = configurationDbContext;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetClient(int id)
    {
        Duende.IdentityServer.EntityFramework.Entities.Client? client = await _clientsServices.GetClientAsync(id);
        
        return Ok(client);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody]CreateClientRequest request)
    {
        string guid = Guid.NewGuid().ToString();
        var client = new Client()
        {
            ClientId = request.ClientName,
            ClientSecrets = new List<Secret>()
            {
                new Secret()
                {
                    Value = guid.Sha256()
                }
            },
            ClientName = request.ClientName,
            AllowedScopes = request.AllowedScopes,
            AllowedGrantTypes = request.AllowedGrantTypes,
        };
        
        var id = await _configurationDbContext.Clients.AddAsync(client.ToEntity());

        return  Ok(guid);
    }
    
    [HttpPut]
    [Route("{Id}")]
    public async Task<IActionResult> UpdateClient(int Id,[FromBody]UpdateClientRequest request)
    {
        var requestToClient = new Client()
        {
            ClientId = request.ClientId,
            AllowedScopes = request.AllowedScopes,
            AllowedGrantTypes = request.AllowedGrantTypes
        };
        var requestEntity = requestToClient.ToEntity();
        var client = await _clientsServices.GetClientAsync(Id);
        if (client is null)
            throw new ApiProblemDetailsException(new ProblemDetails()
            {
                Status = (int?)HttpStatusCode.NotFound,
                Title = "ClientNotFound",
            });
        client.AllowedScopes = requestEntity.AllowedScopes;
        client.AllowedGrantTypes = requestEntity.AllowedGrantTypes;

        var affectedRows = await _clientsServices.UpdateClientsAsync(client , true, true);

        return Ok(affectedRows);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var client = await _clientsServices.GetClientAsync(id);
        
        await _clientsServices.RemoveClientAsync(client);

        return Ok();
    }
}
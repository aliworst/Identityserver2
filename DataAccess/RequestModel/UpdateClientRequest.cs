namespace DataAccess.RequestModel;

public class UpdateClientRequest
{
    public UpdateClientRequest(string clientId, List<string> allowedGrantTypes, List<string> allowedScopes)
    {
        ClientId = clientId;
        AllowedGrantTypes = allowedGrantTypes;
        AllowedScopes = allowedScopes;
    }

    public string ClientId { get; set; }
    public  List<string> AllowedGrantTypes { get; set; }
    public  List<string> AllowedScopes { get; set; }
}


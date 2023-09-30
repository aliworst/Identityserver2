namespace DataAccess.RequestModel;

public class CreateClientRequest
{
    public CreateClientRequest(string? clientName, List<string>? allowedGrantTypes, List<string>? allowedScopes)
    {
        ClientName = clientName;
        AllowedGrantTypes = allowedGrantTypes;
        AllowedScopes = allowedScopes;
    }

    public  string? ClientName { get; set; }
    public  List<string>? AllowedGrantTypes { get; set; }
    public  List<string>? AllowedScopes { get; set; }
}
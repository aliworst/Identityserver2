using System.IdentityModel.Tokens.Jwt;

namespace DataAccess.ResponseModel;

public class CreateConsentResponse
{
    public string ConsentId { get; set; }
    public string Request { get; set; }
}
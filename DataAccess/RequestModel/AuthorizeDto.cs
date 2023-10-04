namespace DataAccess.RequestModel;

public class AuthorizeDto
{
    public string ClientId { get; set; }
    public string redirectUri { get; set; }
    public string responseType { get; set; }
    public string scope { get; set; }
    public string consentId { get; set; }
    public string requestToken { get; set; }
}
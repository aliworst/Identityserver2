using DataAccess.RequestModel;

namespace BusinessRule.Services;

public interface IAuthorizeService
{
    Task<string> AuthorizeLoginService(AuthorizeDto request);
}
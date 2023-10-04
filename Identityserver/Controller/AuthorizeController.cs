using BusinessRule.Services;
using DataAccess.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identityserver.Controller;


[ApiController]
[Route("Api/V1/authorize")]
public class AuthorizeController : ControllerBase
{
    private readonly IAuthorizeService _authorizeService;

    public AuthorizeController(IAuthorizeService authorizeService)
    {
        _authorizeService = authorizeService;
    }
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery]AuthorizeDto request)
    {
        string result = await _authorizeService.AuthorizeLoginService(request);
        return Ok(result);
    }
}
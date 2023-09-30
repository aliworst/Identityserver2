using BusinessRule.Services;
using DataAccess.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace Identityserver.Controller;

[ApiController]
[Route("Api/V1/Consents")]
public class ConsentController : ControllerBase
{
    private readonly IConsentServices _consentServices;

    public ConsentController(IConsentServices consetservices)
    {
        _consentServices = consetservices;
    }

    [HttpPost]
    public async Task<IActionResult> CreateConsent(ConsentDto request)
    {
        var response = _consentServices.CreateConsentAsync(request);
        return Ok(response);
    }
}
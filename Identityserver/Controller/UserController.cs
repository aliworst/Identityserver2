using System.Net;
using AutoWrapper.Exceptions;
using BusinessRule.Services;
using DataAccess.Methods;
using DataAccess.RequestModel;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.Test;
using EntityFramework.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Identityserver.Controller;
[ApiController]
[Route("Api/V1/Users")]
public class UserController : ControllerBase
{
    private readonly IUsersServices _userServices;
    public UserController(IUsersServices userServices)
    {
        _userServices = userServices;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Users>> Get(string id)
    {
        Users? user = await _userServices.GetUserAsync(id);
        
        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Users>> Post([FromBody]UsersDto user)
    {
        var userData = new Users()
        {
            Id = Guid.NewGuid().ToString(),
            Enabled = true,
            Username = user.Username,
            Forename = user.Forename,
            Surname = user.Surname,
            AccountNumber = user.AccountNumber,
            LockoutEnabled = user.LockoutEnabled,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            SecurityStamp = Guid.NewGuid().ToString(),
            TwoFactorEnabled = user.TwoFactorEnabled,
            AccessFailedCount = user.AccessFailedCount,
            LockoutEnd = user.LockoutEnd
        };
        var insertedUser = await _userServices.CreateUserAsync(userData);
        
        return CreatedAtAction(nameof(Get), new { id = insertedUser.Id}, insertedUser);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {

        var user = await _userServices.GetUserAsync(id);
        if (user is null)
            throw new ApiProblemDetailsException(new ProblemDetails()
            {
                Status = (int?)HttpStatusCode.NotFound,
                Title = "UserNotFound",
            });
        var Deleted = await _userServices.RemoveUserAsync(user);

        return Ok(Deleted);
    }
    
    [HttpPut]
    [Route("{Id}")]
    public async Task<IActionResult> Update(String Id,[FromBody]UsersDto user)
    {
        var userToUpdate = await _userServices.GetUserAsync(Id);
        if (userToUpdate is null)
            throw new ApiProblemDetailsException(new ProblemDetails()
            {
                Status = (int?)HttpStatusCode.NotFound,
                Title = "UserNotFound",
            });
        userToUpdate.Username = user.Username;

        var affectedRows = await _userServices.UpdateUsersAsync(userToUpdate);

        return Ok(affectedRows);
    }
}
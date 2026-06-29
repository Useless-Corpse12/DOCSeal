using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using DOCSeal.Application.Users.RegistrationSelf;
using DOCSeal.Application.Users.Authorization;
using DOCSeal.Application.Users.Verification;
using DOCSeal.Application.Users.ChangePassword;
using DOCSeal.Application.Users.RefreshingToken;
using Microsoft.AspNetCore.Authorization;

namespace DOCSeal.Presentation.Controllers;

public class UserController(IMediator mediator) : ApiController
{
    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser(RegistrationSelfCommand cmd)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = await mediator.Send(cmd);
        return Ok(new 
        { 
            UserId = userId, 
            Message = "Регистрация успешна, код подтверждения выслан на почту" 
        });
    }
    
    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> AuthorizeUser(AuthorizationCommand cmd)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var token = await mediator.Send(cmd);
        return Ok(new{Token = token});
    }

    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyUser(VerificationCommand cmd)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = await mediator.Send(cmd);
        return Ok(new 
        { 
            UserId = userId, 
            Message = "Аккаунт верифицирован" 
        });
    }
    
    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> ChangePasswordUser(ChangePasswordCommand cmd)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
        if (string.IsNullOrWhiteSpace(currentUserId))
            return Unauthorized("Не авторизован");
    
        var cmdWithId = cmd with { Id = Guid.Parse(currentUserId) };
    
        var result = await mediator.Send(cmdWithId);
        return Ok(new { Message = result });
    }

    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshUserToken(RefreshTokenCommand cmd)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await mediator.Send(cmd);
        return Ok(new
        {
            result.AccessToken,
            result.RefreshToken
        });
    }
}
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using DOCSeal.Application.Features.Users;
using MediatR;
using DOCSeal.Application.Features.Users.RegistrationSelf;
using DOCSeal.Application.Features.Users.Authorization;
using DOCSeal.Application.Features.Users.Verification;
using DOCSeal.Application.Features.Users.ChangePassword;
using Microsoft.AspNetCore.Authorization;

namespace DOCSeal.Presentation.Controllers;

public class UserController(IMediator mediator) : ApiController
{
    
    private readonly IMediator _mediator = mediator;
    
    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser(RegistrationSelfCommand cmd)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = await _mediator.Send(cmd);
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

        var token = await _mediator.Send(cmd);
        return Ok(new{Token = token});
    }

    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyUser(VerificationCommand cmd)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = await _mediator.Send(cmd);
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
    
        var result = await _mediator.Send(cmdWithId);
        return Ok(new { Message = result });
    }
    
    
    
}
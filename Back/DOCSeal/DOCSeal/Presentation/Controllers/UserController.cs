using Microsoft.AspNetCore.Mvc;
using DOCSeal.Application.Features.Users;
using DOCSeal.Infrastructure.Services.EmailService;

namespace DOCSeal.Presentation.Controllers;

public class UserController : ApiController
{
    [HttpPost("[action]")]
    public async Task<IActionResult> RegistrateUser(RegistrationSelfCommand cmd)
    {
        return Ok("UserRegistered");
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> AuthorizateUser()
    {
        return Ok("UserAuthenticated");
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> VerifyUser()
    {
        return Ok("UserVerified");
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> ChangePassword()
    {
        return Ok("PasswordChanged");
    }
    
    
    
}
using Microsoft.AspNetCore.Mvc;
using DOCSeal.Infrastructure.Services.EmailService;

namespace DOCSeal.Presentation.Controllers;

public class UserController : ApiController
{
    
    [HttpPost("[action]")]
    public async Task<IActionResult> RegisterUser()
    {
        return Ok("UserRegistered");
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> AuthenticateUser()
    {
        return Ok("UserAuthenticated");
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> ChangePassword()
    {
        return Ok("PasswordChanged");
    }
    
    
    
}
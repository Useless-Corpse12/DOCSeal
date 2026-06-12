using Microsoft.AspNetCore.Mvc;

namespace DOCSeal.Controllers;

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
using Microsoft.AspNetCore.Mvc;

namespace DOCSeal.Controllers;

public class OrganisationController : ApiController
{
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateOrganisation()
    {
        return Ok("OrgCreated");
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> DisposeOrganisation()
    {
        return Ok("OrgDisposed");
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> AddEmploee()
    {
        return Ok("OrgChanged");
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> RemoveEmploee()
    {
        return Ok("OrgDisposed");
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> InviteEmploeeToOrganisation()
    {
        return Ok("OrgInviteCodeCreated");
    }
}
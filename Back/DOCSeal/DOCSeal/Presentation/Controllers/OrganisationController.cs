using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using DOCSeal.Application.Orgs;
using Microsoft.AspNetCore.Authorization;

namespace DOCSeal.Presentation.Controllers;

public class OrganizationController(IMediator mediator) : ApiController
{
    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> GetOrganisationInfo([FromBody] GetOrganisationInfoCommand cmd)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserId)) return Unauthorized();

        var cmdWithUser = cmd with { RequesterId = Guid.Parse(currentUserId) };
        var result = await mediator.Send(cmdWithUser);
    
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> CreateMyOrganisation(CreateMyOrganisationCommand cmd)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserId)) return Unauthorized("Не авторизован");

        var cmdWithId = cmd with { OwnerId = Guid.Parse(currentUserId) };
        var orgId = await mediator.Send(cmdWithId);
        
        return Ok(new { OrganisationId = orgId });
    }

    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> GetOrganisations()
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserId)) return Unauthorized("Не авторизован");

        var cmd = new GetOrganisationsCommand(Guid.Parse(currentUserId));
        var result = await mediator.Send(cmd);
        
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> DirectInvite(DirectInviteCommand cmd)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserId)) return Unauthorized("Не авторизован");

        var cmdWithId = cmd with { RequesterId = Guid.Parse(currentUserId) };
        var message = await mediator.Send(cmdWithId);
        
        return Ok(new { Message = message });
    }

    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> InviteLinkGeneration(InviteLinkGenerationCommand cmd)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserId)) return Unauthorized("Не авторизован");

        var cmdWithId = cmd with { RequesterId = Guid.Parse(currentUserId) };
        var inviteCode = await mediator.Send(cmdWithId);
        
        return Ok(new { InviteCode = inviteCode });
    }
    
    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> MultiEmailInvite(MultiEmailInviteCommand cmd)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserId)) return Unauthorized("Не авторизован");

        var cmdWithId = cmd with { RequesterId = Guid.Parse(currentUserId) };
        var sentCount = await mediator.Send(cmdWithId);
        
        return Ok(new { SentCount = sentCount, Message = $"Отправлено {sentCount} приглашений" });
    }

    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> AcceptInvite(AcceptInviteCommand cmd)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserId)) return Unauthorized("Не авторизован");

        var cmdWithId = cmd with { UserId = Guid.Parse(currentUserId) };
        var message = await mediator.Send(cmdWithId);
        
        return Ok(new { Message = message });
    }
    
    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> GetInviteCodes([FromBody] GetInviteCodesCommand cmd)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserId)) return Unauthorized();

        var cmdWithUser = cmd with { RequesterId = Guid.Parse(currentUserId) };
        var result = await mediator.Send(cmdWithUser);
    
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> GetInviteInfo([FromBody] GetInviteInfoCommand cmd)
    {
        var result = await mediator.Send(cmd);
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> DeleteInviteCode([FromBody] DeleteInviteCodeCommand cmd)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserId)) return Unauthorized();

        var cmdWithUser = cmd with { RequesterId = Guid.Parse(currentUserId) };
        var result = await mediator.Send(cmdWithUser);
    
        return Ok(new { Message = result });
    }

    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand cmd)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserId)) return Unauthorized();

        var cmdWithUser = cmd with { RequesterId = Guid.Parse(currentUserId) };
        var result = await mediator.Send(cmdWithUser);
    
        return Ok(new { Message = result });
    }

    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleCommand cmd)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserId)) return Unauthorized();

        var cmdWithUser = cmd with { RequesterId = Guid.Parse(currentUserId) };
        var result = await mediator.Send(cmdWithUser);
    
        return Ok(new { Message = result });
    }

    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> DeleteRole([FromBody] DeleteRoleCommand cmd)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserId)) return Unauthorized();

        var cmdWithUser = cmd with { RequesterId = Guid.Parse(currentUserId) };
        var result = await mediator.Send(cmdWithUser);
    
        return Ok(new { Message = result });
    }
}
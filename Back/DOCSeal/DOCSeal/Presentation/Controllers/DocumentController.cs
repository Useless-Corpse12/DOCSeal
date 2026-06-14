using Microsoft.AspNetCore.Mvc;

namespace DOCSeal.Controllers;

public class DocumentController : ApiController
{
    [HttpPost("[action]")]
    public async Task<IActionResult> AddDocument()
    {
        return Ok("DocAdd");
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> SendDocument()
    {
        return Ok("DocSent");
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> DeleteDocument()
    {
        return Ok("DocDeleted");
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> SignDocument()
    {
        return Ok("DocumentSigned");
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> RejectDocument()
    {
        return Ok("DocumentReject");
    }
}
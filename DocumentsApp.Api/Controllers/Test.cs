using Microsoft.AspNetCore.Mvc;

namespace DocumentsApp.Api.Controllers;

[ApiController]
[Route("Test")]
public class Test: ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok("Success");
    }
}
using Microsoft.AspNetCore.Mvc;

namespace WorldOfTheVoid.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorldController : ControllerBase
{
    [HttpGet("state")]
    [ProducesResponseType<WorldDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWorldState()
    {
        // Implementation to retrieve and return the world state
        return Ok();
    }
}
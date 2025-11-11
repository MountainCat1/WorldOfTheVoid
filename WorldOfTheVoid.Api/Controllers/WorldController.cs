using Microsoft.AspNetCore.Mvc;
using WorldOfTheVoid.Features;

namespace WorldOfTheVoid.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorldController : ControllerBase
{
    private GetWorldStateHandler _getWorldStateHandler;

    public WorldController(GetWorldStateHandler getWorldStateHandler)
    {
        _getWorldStateHandler = getWorldStateHandler;
    }

    [HttpGet("state")]
    [ProducesResponseType<WorldDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWorldState()
    {
        var result = await _getWorldStateHandler.Handle(new GetWorldStateQuery());
        return Ok(result);
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorldOfTheVoid.Dtos;
using WorldOfTheVoid.Extensions;
using WorldOfTheVoid.Features;

namespace WorldOfTheVoid.Controllers;

[ApiController]
[Authorize]
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
        var result = await _getWorldStateHandler.Handle(new GetWorldStateQuery()
        {
            AccountId = User.GetUserContext().AccountId
        });
        return Ok(result);
    }
}
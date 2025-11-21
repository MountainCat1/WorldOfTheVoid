using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorldOfTheVoid.Domain;
using WorldOfTheVoid.Dtos;
using WorldOfTheVoid.Features.Characters;
using WorldOfTheVoid.Services;

namespace WorldOfTheVoid.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CharacterController : ControllerBase
{
    private readonly CreateCharacterHandler _createCharacterHandler;
    
    private readonly GetCharactersHandler _getCharactersHandler;

    private readonly IUserContextService _userContextService;

    public CharacterController(CreateCharacterHandler createCharacterHandler, IUserContextService userContextService, GetCharactersHandler getCharactersHandler)
    {
        _createCharacterHandler = createCharacterHandler;
        _userContextService = userContextService;
        _getCharactersHandler = getCharactersHandler;
    }


    [HttpPost]
    [ProducesResponseType<CharacterDto>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCharacter([FromBody] CreateCharacterRequest request)
    {
        var userContext = _userContextService.GetUserContext();
        
        var command = new CreateCharacterCommand
        {
            AccountId = new EntityId(userContext.AccountId),
            Name = request.Name
        };
        
        var result = await _createCharacterHandler.Handle(command);
        
        return CreatedAtAction(nameof(GetCharacterById), new { id = result.Id }, result);
    }

    [HttpGet]
    [ProducesResponseType<List<CharacterDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCharacters()
    {
        var userContext = _userContextService.GetUserContext();
        
        var query = new GetCharactersQuery
        {
            AccountId = new EntityId(userContext.AccountId)
        };
     
        var result = await _getCharactersHandler.Handle(query);
        
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType<CharacterDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCharacterById([FromRoute] Guid id)
    {
        // Implementation for retrieving a character by ID would go here.
        return StatusCode(StatusCodes.Status501NotImplemented); // Not Implemented
    }
}
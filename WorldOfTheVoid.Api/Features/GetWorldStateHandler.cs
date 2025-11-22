using WorldOfTheVoid.Domain;
using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Dtos;
using WorldOfTheVoid.Interfaces;

namespace WorldOfTheVoid.Features;

public class GetWorldStateQuery : IQuery<WorldDto>
{
    public required EntityId AccountId { get; set; }
}

public class GetWorldStateHandler : IQueryHandler<GetWorldStateQuery, WorldDto>
{
    private IWorldRepository _worldRepository;
    private ICharacterRepository _characterRepository;

    public GetWorldStateHandler(IWorldRepository worldRepository, ICharacterRepository characterRepository)
    {
        _worldRepository = worldRepository;
        _characterRepository = characterRepository;
    }

    public async Task<WorldDto> Handle(GetWorldStateQuery query)
    {
        var world = await _worldRepository.GetDefaultWorld();
        
        var ownCharacters = world.Characters.Where(c => c.AccountId == query.AccountId);
        
        var ownCharactersFullData = await _characterRepository.GetByIds(ownCharacters.Select(c => c.Id).ToList());

        return WorldDto.Create(world, ownCharactersFullData.Select(OwnCharacterDto.Create).ToList());
    }
}
using WorldOfTheVoid.Domain;
using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Dtos;
using WorldOfTheVoid.Interfaces;

namespace WorldOfTheVoid.Features.Characters;

public class GetCharactersQuery : IQuery<ICollection<CharacterDto>>
{
    public EntityId AccountId { get; set; }
}

public class GetCharactersHandler : IQueryHandler<GetCharactersQuery, ICollection<CharacterDto>>
{
    private readonly IWorldRepository _worldRepository;

    public GetCharactersHandler(IWorldRepository worldRepository)
    {
        _worldRepository = worldRepository;
    }

    public async Task<ICollection<CharacterDto>> Handle(GetCharactersQuery query)
    {
        var world = await _worldRepository.GetDefaultWorld();
        
        var playerCharacters = world.Characters.Where(c => c.AccountId == query.AccountId).ToList();
        
        return playerCharacters.Select(CharacterDto.Create).ToList();
    }
}
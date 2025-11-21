using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Interfaces;

namespace WorldOfTheVoid.Features;

public class GetWorldStateQuery : IQuery<WorldDto>
{
}

public class GetWorldStateHandler : IQueryHandler<GetWorldStateQuery, WorldDto>
{
    private IWorldRepository _worldRepository;

    public GetWorldStateHandler(IWorldRepository worldRepository)
    {
        _worldRepository = worldRepository;
    }

    public async Task<WorldDto> Handle(GetWorldStateQuery query)
    {
        var world = await _worldRepository.GetDefaultWorld();

        return WorldDto.Create(world);
    }
}
using WorldOfTheVoid.Domain.Consts;
using WorldOfTheVoid.Domain.Entities;

namespace WorldOfTheVoid.Domain.Ports;

public interface IWorldRepository
{
    public async Task<World> GetDefaultWorld()
    {
        return await GetWorldById(WorldConsts.DefaultWorldId) ?? throw new NullReferenceException("Default world not found");
    }
    
    public Task<World?> GetWorldById(EntityId id);
}
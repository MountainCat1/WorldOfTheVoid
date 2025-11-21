using WorldOfTheVoid.Domain.Consts;
using WorldOfTheVoid.Domain.Entities;

namespace WorldOfTheVoid.Domain.Ports;

public interface IWorldRepository
{
    public async Task<World> GetDefaultWorld(bool asNoTracking = false)
    {
        return await GetWorldById(WorldConsts.DefaultWorldId, asNoTracking) ?? throw new NullReferenceException("Default world not found");
    }
    
    public Task<World?> GetWorldById(EntityId id, bool asNoTracking = false);
    Task<int> SaveChangesAsync();
}
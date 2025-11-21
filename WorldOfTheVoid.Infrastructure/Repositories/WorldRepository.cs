using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Internal;
using WorldOfTheVoid.Domain;
using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Infrastructure.DbContext;

namespace WorldOfTheVoid.Infrastructure.Repositories;

public class WorldRepository : IWorldRepository
{
    private GameDbContext _dbContext;

    public WorldRepository(GameDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<World?> GetWorldById(EntityId id, bool asNoTracking = false)
    {
        if (asNoTracking)
        {
            return await _dbContext.Worlds
                .AsNoTracking()
                .Include(x => x.Characters)
                .Include(x => x.Places)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        return await _dbContext.Worlds
            .Include(x => x.Characters)
            .Include(x => x.Places)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<int> SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }
}
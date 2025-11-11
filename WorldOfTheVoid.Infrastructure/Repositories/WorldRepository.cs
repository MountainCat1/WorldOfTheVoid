using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Internal;
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

    public async Task<World?> GetWorldById(Guid id)
    {
        return await _dbContext.Worlds
            .Include(x => x.Characters)
            .Include(x => x.Places)
            .FirstAsync(x => x.Id == id);
    }
}
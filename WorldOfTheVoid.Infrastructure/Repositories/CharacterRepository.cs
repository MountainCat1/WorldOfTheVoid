using WorldOfTheVoid.Domain;
using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Domain.Entities.Orders;
using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Infrastructure.DbContext;

namespace WorldOfTheVoid.Infrastructure.Repositories;

public class CharacterRepository : ICharacterRepository
{
    private GameDbContext _dbContext;

    public CharacterRepository(GameDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Character?> GetById(EntityId id)
    {
        return await _dbContext.Characters.FindAsync(id);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAllOrders(Character character)
    {
        var orders = _dbContext.Orders.Where(o => o.CharacterId == character.Id);
        _dbContext.Orders.RemoveRange(orders);
        await Task.CompletedTask;
    }
}
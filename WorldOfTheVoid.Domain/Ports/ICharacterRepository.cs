using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Domain.Entities.Orders;

namespace WorldOfTheVoid.Domain.Ports;

public interface ICharacterRepository
{
    Task<Character?> GetById(EntityId id);
    Task<int> SaveChangesAsync();
    Task RemoveAllOrders(Character character);
    Task<ICollection<Character>> GetByIds(List<EntityId> toList);
}
using WorldOfTheVoid.Domain.Entities.Orders;

namespace WorldOfTheVoid.Domain.Ports;

public interface IOrderRepository
{
    public Task<ICollection<Order>> GetAllAsync(bool asNoTracking = false);
    void Remove(Order order);
    Task<int> SaveChangesAsync();
}
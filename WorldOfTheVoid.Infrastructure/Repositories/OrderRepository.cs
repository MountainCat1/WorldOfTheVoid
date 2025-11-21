using Microsoft.EntityFrameworkCore;
using WorldOfTheVoid.Domain.Entities.Orders;
using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Infrastructure.DbContext;

namespace WorldOfTheVoid.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private GameDbContext _dbContext;

    public OrderRepository(GameDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ICollection<Order>> GetAllAsync(bool asNoTracking = false)
    {
        if (asNoTracking)
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .ToListAsync();
        }

        return await _dbContext.Orders
            .ToListAsync();
    }

    public void Remove(Order order)
    {
        _dbContext.Orders.Remove(order);
    }
    
    public Task<int> SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }
}
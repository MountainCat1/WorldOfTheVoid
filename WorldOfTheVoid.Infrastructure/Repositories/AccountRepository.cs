using Microsoft.EntityFrameworkCore;
using WorldOfTheVoid.Domain;
using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Infrastructure.DbContext;

namespace WorldOfTheVoid.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly GameDbContext _dbContext;

    public AccountRepository(GameDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool> IsUsernameTakenAsync(string username)
    {
        return await Task.FromResult(_dbContext.Accounts.Any(a => a.Username == username));
    }
    
    public Task AddAccount(Domain.Entities.Account account)
    {
        _dbContext.Accounts.Add(account);
        return Task.CompletedTask;
    }
    
    public async Task <Domain.Entities.Account?> GetAccountByUsernameAsync(string username)
    {
        return await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Username == username);
    }

    public Task<Account?> GetAccountByIdAsync(EntityId id)
    {
        return _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
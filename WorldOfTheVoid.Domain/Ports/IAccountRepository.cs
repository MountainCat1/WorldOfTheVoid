namespace WorldOfTheVoid.Domain.Ports;

public interface IAccountRepository
{
    Task<bool> IsUsernameTakenAsync(string username);
    Task AddAccount(Entities.Account account);
    Task <Entities.Account?> GetAccountByUsernameAsync(string username);
    Task <Entities.Account?> GetAccountByIdAsync(EntityId id);
    Task<int> SaveChangesAsync();
}
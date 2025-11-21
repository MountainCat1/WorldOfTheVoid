using WorldOfTheVoid.Domain.Entities;

namespace WorldOfTheVoid.Domain.Services;

public interface IAccountService
{
    Task<Account> RegisterAccountAsync(string username, string email, string password);
    Task<LoginResult> LoginAsync(string username, string password);
}

public class AccountService : IAccountService
{
    private readonly Ports.IAccountRepository _accountRepository;

    public AccountService(Ports.IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Account> RegisterAccountAsync(string username, string email, string password)
    {
        if (await _accountRepository.IsUsernameTakenAsync(username))
        {
            throw new InvalidOperationException("Username is already taken.");
        }
        
        var (hash, salt) = PasswordHashing.HashPassword(password);

        var account = Account.Create(username, email, hash, salt);
        
        await _accountRepository.AddAccount(account);
        await _accountRepository.SaveChangesAsync();

        return account;
    }
    
    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        var account = await _accountRepository.GetAccountByUsernameAsync(username);
        if (account == null)
        {
            return new LoginResult { Success = false, Account = null };
        }

        bool isPasswordValid = PasswordHashing.Verify(password, account.PasswordHash, account.PasswordSalt);
        if (!isPasswordValid)
        {
            return new LoginResult { Success = false, Account = null };
        }

        return new LoginResult { Success = true, Account = account };
    }
}

public record LoginResult
{
    public bool Success { get; set; }
    public Account? Account { get; set; }
}
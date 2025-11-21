using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Domain.Services;
using WorldOfTheVoid.Dtos;
using WorldOfTheVoid.Interfaces;

namespace WorldOfTheVoid.Features.Accounts;

public class RegisterAccountCommand : ICommand<AccountDto>
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class RegisterAccountHandler : ICommandHandler<RegisterAccountCommand, AccountDto>
{
    private readonly IAccountService _accountService;

    public RegisterAccountHandler(IAccountService accountService, IAccountRepository accountRepository)
    {
        _accountService = accountService;
    }

    public async Task<AccountDto> Handle(RegisterAccountCommand command)
    {
        var account = await _accountService.RegisterAccountAsync(command.Username, command.Email, command.Password);

        return new AccountDto
        {
            Id = account.Id,
            Username = account.Username,
            Email = account.Email
        };
    }
}
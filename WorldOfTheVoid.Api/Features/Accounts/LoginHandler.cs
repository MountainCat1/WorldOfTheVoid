using System.Security.Claims;
using WorldOfTheVoid.Auth.Consts;
using WorldOfTheVoid.Auth.Services;
using WorldOfTheVoid.Domain.Services;
using WorldOfTheVoid.Interfaces;

namespace WorldOfTheVoid.Features.Accounts;

public class LoginResult
{
    public string? Token { get; set; }
    public bool Success { get; set; }
}

public class LoginCommand : ICommand<LoginResult>
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginHandler : ICommandHandler<LoginCommand, LoginResult>
{
    private IAccountService _accountService;
    private IJwtTokenService _jwtTokenService;

    public LoginHandler(IAccountService accountService, IJwtTokenService jwtTokenService)
    {
        _accountService = accountService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResult> Handle(LoginCommand command)
    {
        var loginResult = await _accountService.LoginAsync(command.Username, command.Password);
        
        if(loginResult.Success == false)
        {
            return new LoginResult
            {
                Success = false,
                Token = null
            };
        }
        
        var claims = new List<Claim>
        {
            new(AccountClaims.AccountId, loginResult.Account!.Id.ToString()),
            new(AccountClaims.Username, loginResult.Account.Username),
            new(AccountClaims.Email, loginResult.Account.Email)
        };
        
        var token = _jwtTokenService.GenerateToken(claims);

        return new LoginResult
        {
            Token = token,
            Success = true
        };
    }
}
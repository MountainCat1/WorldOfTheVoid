using Microsoft.AspNetCore.Mvc;
using WorldOfTheVoid.Features.Accounts;
using LoginRequest = WorldOfTheVoid.Dtos.LoginRequest;
using RegisterRequest = WorldOfTheVoid.Dtos.RegisterRequest;

namespace WorldOfTheVoid.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RegisterAccountHandler _registerAccountHandler;
    private LoginHandler _loginHandler;

    public AuthController(RegisterAccountHandler registerAccountHandler, LoginHandler loginHandler)
    {
        _registerAccountHandler = registerAccountHandler;
        _loginHandler = loginHandler;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = new RegisterAccountCommand
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password
        };

        var result = await _registerAccountHandler.Handle(command);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand
        {
            Username = request.Username,
            Password = request.Password
        };

        var result = await _loginHandler.Handle(command);

        return Ok(result);
    }
}
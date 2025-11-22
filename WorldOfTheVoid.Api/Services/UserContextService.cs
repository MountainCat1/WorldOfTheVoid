using WorldOfTheVoid.Auth.Consts;
using WorldOfTheVoid.Domain;
using WorldOfTheVoid.Extensions;

namespace WorldOfTheVoid.Services;

public record UserContext
{
    public required EntityId AccountId { get; init; }
    public required string Username { get; init; }
}

public interface IUserContextService
{
    string? UserId { get; }
    string? Username { get; }
    UserContext GetUserContext();
}

public sealed class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _accessor;

    public UserContextService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }
    
    public UserContext GetUserContext()
    {
        return _accessor.HttpContext?.User?.GetUserContext() ;
    }

    public string? UserId =>
        _accessor.HttpContext?.User.FindFirst(AccountClaims.AccountId)?.Value;

    public string? Username =>
        _accessor.HttpContext?.User.FindFirst(AccountClaims.Username)?.Value;
}

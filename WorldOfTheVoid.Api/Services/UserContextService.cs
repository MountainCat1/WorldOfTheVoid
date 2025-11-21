using WorldOfTheVoid.Auth.Consts;

namespace WorldOfTheVoid.Services;

public record UserContext
{
    public required string AccountId { get; init; }
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
        var userId = _accessor.HttpContext?.User.FindFirst(AccountClaims.AccountId)?.Value;
        var username = _accessor.HttpContext?.User.FindFirst(AccountClaims.Username)?.Value;

        if (userId == null || username == null)
        {
            throw new InvalidOperationException("User is not authenticated.");
        }

        return new UserContext
        {
            AccountId = userId,
            Username = username
        };
    }

    public string? UserId =>
        _accessor.HttpContext?.User.FindFirst(AccountClaims.AccountId)?.Value;

    public string? Username =>
        _accessor.HttpContext?.User.FindFirst(AccountClaims.Username)?.Value;
}

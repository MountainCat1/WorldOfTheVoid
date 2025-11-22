using System.Security.Claims;
using WorldOfTheVoid.Auth.Consts;
using WorldOfTheVoid.Services;

namespace WorldOfTheVoid.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static UserContext GetUserContext(this ClaimsPrincipal? claims)
    {
        if (claims == null)
        {
            throw new InvalidOperationException("User is not authenticated, or claims principal is null.");
        }
        
        var userId = claims.FindFirst(AccountClaims.AccountId)?.Value;
        var username = claims.FindFirst(AccountClaims.Username)?.Value;

        if (userId == null || username == null)
        {
            throw new InvalidOperationException("User is not authenticated.");
        }

        return new UserContext
        {
            AccountId = new Domain.EntityId(userId),
            Username = username
        };
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WorldOfTheVoid.Auth.Settings;

namespace WorldOfTheVoid.Auth.Services;

public interface IJwtTokenService
{
    string GenerateToken(List<Claim> claims);
}

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _options;
    private readonly byte[] _keyBytes;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
        _keyBytes = Encoding.UTF8.GetBytes(_options.Key);
    }

    public string GenerateToken(List<Claim> claims)
    {
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(_keyBytes),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiresMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
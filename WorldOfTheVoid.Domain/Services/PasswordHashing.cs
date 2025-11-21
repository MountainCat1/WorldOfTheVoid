using System.Security.Cryptography;

namespace WorldOfTheVoid.Domain.Services;

public static class PasswordHashing
{
    private const int SaltSize = 16;        // 128-bit
    private const int KeySize = 32;         // 256-bit
    private const int Iterations = 100_000; // strong PBKDF2

    public static (string Hash, string Salt) HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);

        var key = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySize);

        return (
            Convert.ToBase64String(key),
            Convert.ToBase64String(salt)
        );
    }

    public static bool Verify(string password, string storedHash, string storedSalt)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);
        var storedKeyBytes = Convert.FromBase64String(storedHash);

        var derivedKey = Rfc2898DeriveBytes.Pbkdf2(
            password,
            saltBytes,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySize);

        return CryptographicOperations.FixedTimeEquals(derivedKey, storedKeyBytes);
    }
}

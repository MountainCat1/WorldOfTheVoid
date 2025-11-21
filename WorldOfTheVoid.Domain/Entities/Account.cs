namespace WorldOfTheVoid.Domain.Entities;

public class Account
{
    public EntityId Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }

    private Account()
    {
    }
    
    public static Account Create(string username, string email, string passwordHash, string passwordSalt)
    {
        return new Account
        {
            Id = EntityId.Create(EntityType.Account),
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
    }
}
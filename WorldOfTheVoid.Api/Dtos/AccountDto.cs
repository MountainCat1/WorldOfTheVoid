using WorldOfTheVoid.Domain;

namespace WorldOfTheVoid.Dtos;

public class AccountDto
{
    public EntityId Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}
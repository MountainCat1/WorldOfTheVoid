using System.Numerics;
using System.Text.Json.Serialization;

namespace WorldOfTheVoid.Domain.Entities;

public class Character
{
    public EntityId Id { get; set; }
    public string Name { get; set; }
    public Vector3 Position { get; set; }

    public EntityId WorldId { get; set; }
    [JsonIgnore]
    public virtual World World { get; set; }
    
    public EntityId AccountId { get; set; }
    [JsonIgnore]
    public virtual Account Owner { get; set; }

    public static Character Create(Account account, string name)
    {
        return new Character
        {
            Id = EntityId.Create(EntityType.Character),
            Name = name,
            Position = Vector3.Zero,
            AccountId = account.Id,
            Owner = account,
        };
    }
}
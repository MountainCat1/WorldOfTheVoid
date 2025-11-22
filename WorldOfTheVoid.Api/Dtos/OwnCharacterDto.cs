using System.Numerics;
using System.Reflection;
using System.Text.Json.Serialization;
using WorldOfTheVoid.Domain;
using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Domain.Entities.Orders;

namespace WorldOfTheVoid.Dtos;

public class OwnCharacterDto
{
    public EntityId Id { get; set; }
    public string Name { get; set; }
    public Vector3 Position { get; set; }

    public EntityId WorldId { get; set; }
    public EntityId AccountId { get; set; }
    
    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public static OwnCharacterDto Create(Character? world)
    {
        if (world == null)
            throw new ArgumentNullException(nameof(world));

        var dto = new OwnCharacterDto();
        var sourceType = typeof(Character);
        var targetType = typeof(OwnCharacterDto);

        foreach (var prop in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!prop.CanRead)
                continue;

            var targetProp = targetType.GetProperty(prop.Name, BindingFlags.Public | BindingFlags.Instance);
            if (targetProp == null || !targetProp.CanWrite)
                continue;

            var value = prop.GetValue(world);
            targetProp.SetValue(dto, value);
        }

        return dto;
    }
    
}
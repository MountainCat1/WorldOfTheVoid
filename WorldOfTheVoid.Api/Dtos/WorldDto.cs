using System.Reflection;
using WorldOfTheVoid.Domain.Entities;

public class WorldDto : World
{
    public static WorldDto Create(World? world)
    {
        if (world == null)
            throw new ArgumentNullException(nameof(world));

        var dto = new WorldDto();
        var sourceType = typeof(World);
        var targetType = typeof(WorldDto);

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
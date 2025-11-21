using System.Reflection;
using WorldOfTheVoid.Domain.Entities;

namespace WorldOfTheVoid.Dtos;

public class CharacterDto : Character
{
    public static CharacterDto Create(Character? world)
    {
        if (world == null)
            throw new ArgumentNullException(nameof(world));

        var dto = new CharacterDto();
        var sourceType = typeof(Character);
        var targetType = typeof(CharacterDto);

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
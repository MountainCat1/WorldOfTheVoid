namespace WorldOfTheVoid.Domain;

public static class IdGenerator
{
    public static string CreateId(EntityType entityType)
        => $"{entityType}_{Guid.NewGuid()}";

    public static (EntityType EntityType, Guid Guid) Parse(string id)
    {
        var parts = id.Split('_', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2 || !Guid.TryParse(parts[1], out var guid))
            throw new FormatException($"Invalid ID format: {id}");

        if (!Enum.TryParse(parts[0], out EntityType type))
            throw new FormatException($"Unknown entity type: {parts[0]}");

        return (type, guid);
    }
}
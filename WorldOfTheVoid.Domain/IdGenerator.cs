namespace WorldOfTheVoid.Domain;

public enum EntityType
{
    Character,
    Place,
    World
}

public readonly struct EntityId : IEquatable<EntityId>
{
    public string Value { get; }
    public EntityType Type { get; }
    public Guid Guid { get; }

    public EntityId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("EntityId value cannot be null or empty.", nameof(value));

        var (type, guid) = IdGenerator.Parse(value);
        Value = value;
        Type = type;
        Guid = guid;
    }

    public override string ToString() => Value;

    public bool Equals(EntityId other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is EntityId other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    
    public static bool operator ==(EntityId left, EntityId right) => left.Equals(right);
    public static bool operator !=(EntityId left, EntityId right) => !left.Equals(right);

    public static EntityId Create(EntityType type) => new(IdGenerator.CreateId(type));
    public static EntityId Create(EntityType type, string rawId) => new($"{type}_{rawId}");
}

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
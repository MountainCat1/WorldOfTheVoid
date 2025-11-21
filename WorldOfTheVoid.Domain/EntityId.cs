namespace WorldOfTheVoid.Domain;

public enum EntityType
{
    Character,
    Place,
    World,
    Account,
    Order
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
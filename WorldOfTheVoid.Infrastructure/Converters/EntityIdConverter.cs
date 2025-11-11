using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorldOfTheVoid.Domain;
using WorldOfTheVoid.Domain.Entities;

namespace WorldOfTheVoid.Infrastructure.Converters;

public class EntityIdConverter : ValueConverter<EntityId, string>
{
    public EntityIdConverter()
        : base(
            id => id.Value,
            value => new EntityId(value))
    {
    }
}
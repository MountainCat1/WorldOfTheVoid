namespace WorldOfTheVoid.Utilities;

using Domain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class EntityIdJsonConverter : JsonConverter<EntityId>
{
    public override EntityId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return new EntityId(value!);
    }

    public override void Write(Utf8JsonWriter writer, EntityId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}
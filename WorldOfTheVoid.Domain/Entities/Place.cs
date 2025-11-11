using System.Numerics;
using System.Text.Json.Serialization;

namespace WorldOfTheVoid.Domain.Entities;

public class Place
{
    public EntityId Id { get; set; }
    public string Name { get; set; }
    public Vector3 Position { get; set; }
    public int Population { get; set; }


    public EntityId WorldId { get; set; }
    [JsonIgnore]
    public virtual World World { get; set; }
}
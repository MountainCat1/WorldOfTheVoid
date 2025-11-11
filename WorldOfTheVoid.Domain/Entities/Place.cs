using System.Numerics;
using System.Text.Json.Serialization;

namespace WorldOfTheVoid.Domain.Entities;

public class Place
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Vector3 Position { get; set; }
    
    public Guid WorldId { get; set; }
    [JsonIgnore]
    public virtual World World { get; set; }
}
using System.Numerics;

namespace WorldOfTheVoid.Domain.Entities;

public class Character
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Vector3 Position { get; set; }

    public Guid WorldId { get; set; }
    public virtual World World { get; set; }
}
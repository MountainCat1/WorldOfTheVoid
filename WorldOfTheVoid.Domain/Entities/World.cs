namespace WorldOfTheVoid.Domain.Entities;

public class World
{
    public EntityId Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Character> Characters { get; set; }
    public virtual ICollection<Place> Places { get; set; }
}
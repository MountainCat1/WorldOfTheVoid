using WorldOfTheVoid.Domain;

namespace WorldOfTheVoid.Errors;

public class EntityNotFound<T> : Exception
{ 
    public EntityNotFound(string message)
        : base(message)
    {
    }
    
    public EntityNotFound()
        : base($"Entity of type {typeof(T).Name} was not found.")
    {
    }
    
    public EntityNotFound(EntityId id)
        : base($"Entity of type {typeof(T).Name} with ID {id} was not found.")
    {
    }
}
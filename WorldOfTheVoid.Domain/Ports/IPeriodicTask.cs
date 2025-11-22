namespace WorldOfTheVoid.Interfaces;

public interface IPeriodicTask
{
    /// <summary>
    /// Logic to execute.
    /// </summary>
    Task ExecuteAsync(CancellationToken cancellationToken);
}


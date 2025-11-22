namespace WorldOfTheVoid.Infrastructure.DbEntities;

public class PeriodicWorkerLog
{
    public Guid Id { get; set; }
    public DateTimeOffset DateStarted { get; set; }
    public float Time { get; set; }
}
using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Domain.Entities.Orders;
using WorldOfTheVoid.Infrastructure.DbConfiguration;
using WorldOfTheVoid.Infrastructure.DbEntities;

namespace WorldOfTheVoid.Infrastructure.DbContext;

using Microsoft.EntityFrameworkCore;

public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options)
        : base(options) { }

    public DbSet<Character> Characters { get; set; } = null!;
    public DbSet<World> Worlds { get; set; } = null!;
    public DbSet<Place> Places { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    
    
    public DbSet<Account> Accounts { get; set; } = null!;
    
    public DbSet<PeriodicWorkerLog> PeriodicWorkerLogs { get; set; } = null!;
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureGameContext();
    }
}

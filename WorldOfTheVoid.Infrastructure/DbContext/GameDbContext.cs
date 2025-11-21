using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Infrastructure.DbConfiguration;

namespace WorldOfTheVoid.Infrastructure.DbContext;

using Microsoft.EntityFrameworkCore;

public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options)
        : base(options) { }

    public DbSet<Character> Characters { get; set; } = null!;
    public DbSet<World> Worlds { get; set; } = null!;
    public DbSet<Place> Places { get; set; } = null!;
    
    public DbSet<Account> Accounts { get; set; } = null!;
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureGameContext();
    }
}

using Microsoft.EntityFrameworkCore;
using WorldOfTheVoid.Domain.Consts;
using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Infrastructure.DbContext;

namespace WorldOfTheVoid.Infrastructure.DbConfiguration;

public static class GameDbContextConfiguration
{
    public static void ConfigureGameContext(this ModelBuilder mb)
    {
        // Character
        var characterConfig = mb.Entity<Character>();

        characterConfig.HasKey(c => c.Id);
        characterConfig.Property(c => c.Name).IsRequired().HasMaxLength(128);
        characterConfig.Property(c => c.Position)
            .HasConversion(new Vector3Converter())
            .HasMaxLength(64)
            .IsRequired();


        // Place
        var placeConfig = mb.Entity<Place>();

        placeConfig.HasKey(p => p.Id);
        placeConfig.Property(p => p.Name).IsRequired().HasMaxLength(128);
        placeConfig.Property(p => p.Position)
            .HasConversion(new Vector3Converter())
            .HasMaxLength(64)
            .IsRequired();

        // World
        var worldConfig = mb.Entity<World>();
        worldConfig.HasKey(w => w.Id);
        worldConfig.Property(w => w.Name).IsRequired().HasMaxLength(128);
        worldConfig.HasMany<Character>(w => w.Characters)
            .WithOne(c => c.World)
            .HasForeignKey(c => c.WorldId)
            .OnDelete(DeleteBehavior.Cascade);
        worldConfig.HasMany<Place>(w => w.Places)
            .WithOne(p => p.World)
            .HasForeignKey(p => p.WorldId)
            .OnDelete(DeleteBehavior.Cascade);

        // --- SEED DATA ---
        worldConfig.HasData(
            new World
            {
                Id = WorldConsts.DefaultWorldId,
                Name = "The Void"
            }
        );
    }
}
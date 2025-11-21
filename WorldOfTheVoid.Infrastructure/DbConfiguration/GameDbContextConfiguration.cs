using Microsoft.EntityFrameworkCore;
using WorldOfTheVoid.Domain;
using WorldOfTheVoid.Domain.Consts;
using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Infrastructure.Converters;

namespace WorldOfTheVoid.Infrastructure.DbConfiguration;

public static class GameDbContextConfiguration
{
    public static void ConfigureGameContext(this ModelBuilder mb)
    {
        var idConverter = new EntityIdConverter();

        // === ACCOUNT ===
        var accountConfig = mb.Entity<Account>();
        accountConfig.HasKey(a => a.Id);
        accountConfig.Property(a => a.Id)
            .HasConversion(idConverter)
            .HasMaxLength(64)
            .IsRequired()
            .ValueGeneratedNever();
        accountConfig.Property(a => a.Username)
            .IsRequired()
            .HasMaxLength(64);
        accountConfig.Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(128);
        accountConfig.Property(a => a.PasswordHash)
            .IsRequired()
            .HasMaxLength(256);
        accountConfig.Property(a => a.PasswordSalt)
            .IsRequired()
            .HasMaxLength(256);
        
        
        // === CHARACTER ===
        var characterConfig = mb.Entity<Character>();
        characterConfig.HasKey(c => c.Id);

        characterConfig.Property(c => c.Id)
            .HasConversion(idConverter)
            .HasMaxLength(64)
            .IsRequired()
            .ValueGeneratedNever();

        characterConfig.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(128);

        characterConfig.Property(c => c.Position)
            .HasConversion(new Vector3Converter())
            .HasMaxLength(64)
            .IsRequired();

        characterConfig.Property(c => c.WorldId)
            .HasConversion(idConverter)
            .HasMaxLength(64)
            .IsRequired()
            .ValueGeneratedNever();
        
        characterConfig.Property(c => c.AccountId)
            .HasConversion(idConverter)
            .HasMaxLength(64)
            .IsRequired()
            .ValueGeneratedNever();

        // === PLACE ===
        var placeConfig = mb.Entity<Place>();
        placeConfig.HasKey(p => p.Id);

        placeConfig.Property(p => p.Id)
            .HasConversion(idConverter)
            .HasMaxLength(64)
            .IsRequired()
            .ValueGeneratedNever();

        placeConfig.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(128);

        placeConfig.Property(p => p.Position)
            .HasConversion(new Vector3Converter())
            .HasMaxLength(64)
            .IsRequired();

        placeConfig.Property(p => p.WorldId)
            .HasConversion(idConverter)
            .HasMaxLength(64)
            .IsRequired()
            .ValueGeneratedNever();


        // === WORLD ===
        var worldConfig = mb.Entity<World>();
        worldConfig.HasKey(w => w.Id);

        worldConfig.Property(w => w.Id)
            .HasConversion(idConverter)
            .HasMaxLength(64)
            .IsRequired()
            .ValueGeneratedNever();

        worldConfig.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(128);

        worldConfig.HasMany(w => w.Characters)
            .WithOne(c => c.World)
            .HasForeignKey(c => c.WorldId)
            .OnDelete(DeleteBehavior.Cascade);

        worldConfig.HasMany(w => w.Places)
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

        // === GLOBAL RULE: disable key generation for all EntityId keys ===
        foreach (var entityType in mb.Model.GetEntityTypes())
        {
            var idProperty = entityType.FindProperty("Id");
            if (idProperty?.ClrType == typeof(EntityId))
                idProperty.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.Never;
        }
    }
}

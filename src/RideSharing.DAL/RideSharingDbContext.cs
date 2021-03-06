using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;
using RideSharing.DAL.Seeds;

namespace RideSharing.DAL;

public class RideSharingDbContext : DbContext
{
    private readonly bool _seedDemoData;

    public RideSharingDbContext(DbContextOptions contextOptions, bool seedDemoData = false) : base(contextOptions)
    {
        _seedDemoData = seedDemoData;
    }

    public DbSet<ReservationEntity> ReservationEntities => Set<ReservationEntity>();
    public DbSet<ReviewEntity> ReviewEntities => Set<ReviewEntity>();
    public DbSet<RideEntity> RideEntities => Set<RideEntity>();
    public DbSet<UserEntity> UserEntities => Set<UserEntity>();
    public DbSet<VehicleEntity> VehicleEntities => Set<VehicleEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasMany(i => i.Reservations)
                .WithOne(i => i.ReservingUser)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(i => i.SubmittedReviews)
                .WithOne(i => i.AuthorUser)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(i => i.Vehicles)
                .WithOne(i => i.Owner)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RideEntity>(entity =>
        {
            entity.HasMany(i => i.Reservations)
                .WithOne(i => i.Ride)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(i => i.Reviews)
                .WithOne(i => i.Ride)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<VehicleEntity>()
            .HasMany(i => i.Rides)
            .WithOne(i => i.Vehicle)
            .OnDelete(DeleteBehavior.ClientSetNull);

        if (_seedDemoData)
        {
            ReservationSeeds.Seed(modelBuilder);
            VehicleSeeds.Seed(modelBuilder);
            ReviewSeeds.Seed(modelBuilder);
            RideSeeds.Seed(modelBuilder);
            UserSeeds.Seed(modelBuilder);
        }
    }
}

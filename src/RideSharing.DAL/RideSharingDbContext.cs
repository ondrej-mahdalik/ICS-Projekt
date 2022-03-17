using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

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

        modelBuilder.Entity<RideEntity>()
            .HasMany(i => i.Reservations)
            .WithOne(i => i.Ride)
            .OnDelete(DeleteBehavior.Cascade); // When a ride is deleted, we want to delete its reservations as well

        modelBuilder.Entity<UserEntity>()
            .HasMany(i => i.Reviews)
            .WithOne(i => i.User)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEntity>()
            .HasMany(i => i.Vehicles)
            .WithOne(i => i.Owner)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEntity>()
            .HasMany(i => i.Reservations)
            .WithOne(i => i.User)
            .OnDelete(DeleteBehavior.Cascade);

        //modelBuilder.Entity<VehicleEntity>()
        //    .HasOne(i => i.Owner)
        //    .WithMany(i => i.Vehicles)
        //    .OnDelete(DeleteBehavior.Restrict);

        if (_seedDemoData)
        {
            // TODO
        }
    }
}

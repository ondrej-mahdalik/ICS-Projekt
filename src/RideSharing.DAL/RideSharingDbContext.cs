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

        modelBuilder.Entity<UserEntity>()
            .HasMany(i => i.Reservations)
            .WithOne(i => i.ReservingUser)
            .OnDelete(DeleteBehavior.Cascade); // User deletion causes deletion of all his reservations

        modelBuilder.Entity<UserEntity>()
            .HasMany(i => i.Reviews)
            .WithOne(i => i.ReviewedUser)
            .OnDelete(DeleteBehavior.Cascade); // User deletion causes deletion of all reviews that the user obtained

        modelBuilder.Entity<UserEntity>()
            .HasMany(i => i.SubmittedReviews)
            .WithOne(i => i.AuthorUser)
            .OnDelete(DeleteBehavior.ClientSetNull); // User deletion keeps all reviews that the user created

        modelBuilder.Entity<UserEntity>()
            .HasMany(i => i.Vehicles)
            .WithOne(i => i.Owner)
            .OnDelete(DeleteBehavior.Cascade); // User deletion causes deletion of all his vehicles

        modelBuilder.Entity<RideEntity>()
            .HasMany(i => i.Reservations)
            .WithOne(i => i.Ride)
            .OnDelete(DeleteBehavior.Cascade); // Ride deletion causes deletion of all it's reservations

        modelBuilder.Entity<RideEntity>()
            .HasMany(i => i.Reviews)
            .WithOne(i => i.Ride)
            .OnDelete(DeleteBehavior.ClientSetNull); // Ride deletion keeps all reviews regarding that ride

        modelBuilder.Entity<VehicleEntity>()
            .HasMany(i => i.Rides)
            .WithOne(i => i.Vehicle)
            .OnDelete(DeleteBehavior.Restrict); // Can't delete vehicle used in rides

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

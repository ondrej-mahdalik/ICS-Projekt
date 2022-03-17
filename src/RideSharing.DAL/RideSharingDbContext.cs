using RideSharing.DAL.Entities;
using RideSharing.DAL.Seeds;
using Microsoft.EntityFrameworkCore;


namespace RideSharing.DAL
{
    public class RideSharingDbContext : DbContext
    {
        private readonly bool _seedDemoData;

        public RideSharingDbContext(DbContextOptions contextOptions, bool seedDemoData = false)
            : base(contextOptions)
        {
            _seedDemoData = seedDemoData;
        }

        public DbSet<ReservationEntity> Reservations => Set<ReservationEntity>();
        public DbSet<ReviewEntity> Reviews => Set<ReviewEntity>();
        public DbSet<RideEntity> Rides => Set<RideEntity>();
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<VehicleEntity> Vehicles => Set<VehicleEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .HasMany(i => i.Vehicles)
                .WithOne(i => i.Owner)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserEntity>()
                .HasMany(i => i.Reservations)
                .WithOne(i => i.User)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserEntity>()
                .HasMany(i => i.Reviews)
                .WithOne(i => i.User)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<RideEntity>()
                .HasOne(i => i.Driver)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RideEntity>()
                .HasOne(i => i.Vehicle)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RideEntity>()
                .HasMany(i => i.Reservations)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RideEntity>()
                .HasMany(i => i.Reviews)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            if (_seedDemoData)
            {
                ReservationSeeds.Seed(modelBuilder);
                ReviewSeeds.Seed(modelBuilder);
                RideSeeds.Seed(modelBuilder);
                UserSeeds.Seed(modelBuilder);
                VehicleSeeds.Seed(modelBuilder);
            }
        }
    }
}

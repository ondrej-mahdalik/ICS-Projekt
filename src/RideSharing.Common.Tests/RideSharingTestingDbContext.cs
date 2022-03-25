using RideSharing.Common.Tests.Seeds;
using RideSharing.DAL;
using Microsoft.EntityFrameworkCore;

namespace RideSharing.Common.Tests;

public class RideSharingTestingDbContext : RideSharingDbContext
{
    private readonly bool _seedTestingData;

    public RideSharingTestingDbContext(DbContextOptions contextOptions, bool seedTestingData = false)
        : base(contextOptions, seedDemoData:false)
    {
        _seedTestingData = seedTestingData;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (_seedTestingData)
        {
            ReservationSeeds.Seed(modelBuilder);
            ReviewSeeds.Seed(modelBuilder);
            RideSeeds.Seed(modelBuilder);
            VehicleSeeds.Seed(modelBuilder);
            UserSeeds.Seed(modelBuilder);
        }
    }
}

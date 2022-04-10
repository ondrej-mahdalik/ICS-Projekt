using RideSharing.Common.Tests.Seeds;
using DALSeeds = RideSharing.Common.Tests.Seeds;

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
            Seeds.UserSeeds.Seed(modelBuilder);
            Seeds.ReservationSeeds.Seed(modelBuilder);
            Seeds.ReviewSeeds.Seed(modelBuilder);
            Seeds.RideSeeds.Seed(modelBuilder);
            Seeds.VehicleSeeds.Seed(modelBuilder);
        }
        
    }
}

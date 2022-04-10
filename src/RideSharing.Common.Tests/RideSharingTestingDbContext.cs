using Microsoft.EntityFrameworkCore;
using RideSharing.DAL;
using DALSeeds = RideSharing.Common.Tests.Seeds;

namespace RideSharing.Common.Tests;

public class RideSharingTestingDbContext : RideSharingDbContext
{
    private readonly bool _seedTestingData;

    public RideSharingTestingDbContext(DbContextOptions contextOptions, bool seedTestingData = false)
        : base(contextOptions, false)
    {
        _seedTestingData = seedTestingData;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (_seedTestingData)
        {
            DALSeeds.UserSeeds.Seed(modelBuilder);
            DALSeeds.ReservationSeeds.Seed(modelBuilder);
            DALSeeds.ReviewSeeds.Seed(modelBuilder);
            DALSeeds.RideSeeds.Seed(modelBuilder);
            DALSeeds.VehicleSeeds.Seed(modelBuilder);
        }
    }
}

using RideSharing.Common.Tests.Seeds;
using DALSeeds = RideSharing.Common.Tests.DALTestsSeeds;

using RideSharing.DAL;
using Microsoft.EntityFrameworkCore;

namespace RideSharing.Common.Tests;

public class RideSharingTestingDbContext : RideSharingDbContext
{
    private readonly bool _seedDALTestingData;
    private readonly bool _seedBLTestingData;

    public RideSharingTestingDbContext(DbContextOptions contextOptions, bool seedDALTestingData = false, bool seedBLTestingData = false)
        : base(contextOptions, seedDemoData:false)
    {
        _seedDALTestingData = seedDALTestingData;
        _seedBLTestingData = seedBLTestingData;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (_seedDALTestingData)
        {
            DALSeeds.UserSeeds.Seed(modelBuilder);
            DALSeeds.ReservationSeeds.Seed(modelBuilder);
            DALSeeds.ReviewSeeds.Seed(modelBuilder);
            DALSeeds.RideSeeds.Seed(modelBuilder);
            DALSeeds.VehicleSeeds.Seed(modelBuilder);
        }

        if (_seedBLTestingData)
        {
            UserSeeds.Seed(modelBuilder);
            ReservationSeeds.Seed(modelBuilder);
            ReviewSeeds.Seed(modelBuilder);
            RideSeeds.Seed(modelBuilder);
            VehicleSeeds.Seed(modelBuilder);
        }


    }
}

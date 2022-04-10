using Microsoft.EntityFrameworkCore;
using RideSharing.DAL;

namespace RideSharing.Common.Tests.Factories;

public class DbContextTestingInMemoryFactory : IDbContextFactory<RideSharingDbContext>
{
    private readonly string _databaseName;
    private readonly bool _seedTestingData;

    public DbContextTestingInMemoryFactory(string databaseName, bool seedTestingData = false)
    {
        _databaseName = databaseName;
        _seedTestingData = seedTestingData;
    }

    public RideSharingDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<RideSharingDbContext> contextOptionsBuilder = new();
        contextOptionsBuilder.UseInMemoryDatabase(_databaseName);

        // contextOptionsBuilder.LogTo(System.Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
        // builder.EnableSensitiveDataLogging();

        return new RideSharingTestingDbContext(contextOptionsBuilder.Options, _seedTestingData);
    }
}

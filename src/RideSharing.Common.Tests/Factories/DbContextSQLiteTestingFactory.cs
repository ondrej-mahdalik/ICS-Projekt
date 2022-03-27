using RideSharing.DAL;
using Microsoft.EntityFrameworkCore;

namespace RideSharing.Common.Tests.Factories;

public class DbContextSQLiteTestingFactory : IDbContextFactory<RideSharingDbContext>
{
    private readonly string _databaseName;
    private readonly bool _seedDALTestingData;
    private readonly bool _seedBLTestingData;

    public DbContextSQLiteTestingFactory(string databaseName, bool seedDALTestingData = false, bool seedBLTestingData = false)
    {
        _databaseName = databaseName;
        _seedDALTestingData = seedDALTestingData;
        _seedBLTestingData = seedBLTestingData;
    }

    public RideSharingDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<RideSharingDbContext> builder = new();
        builder.UseSqlite($"Data Source={_databaseName};Cache=Shared");
        
         //builder.LogTo(System.Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
         //builder.EnableSensitiveDataLogging();
        
        return new RideSharingTestingDbContext(builder.Options, _seedDALTestingData, _seedBLTestingData);
    }
}

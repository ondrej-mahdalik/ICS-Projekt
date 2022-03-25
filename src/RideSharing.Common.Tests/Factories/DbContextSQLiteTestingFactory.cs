using RideSharing.DAL;
using Microsoft.EntityFrameworkCore;

namespace RideSharing.Common.Tests.Factories;

public class DbContextSQLiteTestingFactory : IDbContextFactory<RideSharingDbContext>
{
    private readonly string _databaseName;
    private readonly bool _seedTestingData;

    public DbContextSQLiteTestingFactory(string databaseName, bool seedTestingData = false)
    {
        _databaseName = databaseName;
        _seedTestingData = seedTestingData;
    }

    public RideSharingDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<RideSharingDbContext> builder = new();
        builder.UseSqlite($"Data Source={_databaseName};Cache=Shared");
        
         //builder.LogTo(System.Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
         //builder.EnableSensitiveDataLogging();
        
        return new RideSharingTestingDbContext(builder.Options, _seedTestingData);
    }
}

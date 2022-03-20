using Microsoft.EntityFrameworkCore;

namespace RideSharing.DAL.Factories
{
    public class SqlServerDbContextFactory : IDbContextFactory<RideSharingDbContext>
    {
        private readonly string _connectionString;
        private readonly bool _seedDemoData;

        public SqlServerDbContextFactory(string connectionString, bool seedDemoData = false)
        {
            _connectionString = connectionString;
            _seedDemoData = seedDemoData;
        }

        public RideSharingDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<RideSharingDbContext>();
            optionsBuilder.UseSqlServer(_connectionString);

            //optionsBuilder.LogTo(System.Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
            //optionsBuilder.EnableSensitiveDataLogging();

            return new RideSharingDbContext(optionsBuilder.Options, _seedDemoData);
        }
    }
}

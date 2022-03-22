using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RideSharing.DAL.Factories
{
    /// <summary>
    /// EF Core CLI migration generation uses this DbContext to create model and migration
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RideSharingDbContext>
    {
        public RideSharingDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<RideSharingDbContext> builder = new();
            builder.UseSqlServer(
                @"Data Source=(LocalDB)\MSSQLLocalDB;
                Initial Catalog = RideSharing;
                MultipleActiveResultSets = True;
                Integrated Security = True; "); // Connection string will be moved into AppSettings.json when RideSahring.App will be created (in 3. phase of development)

            return new RideSharingDbContext(builder.Options);
        }
    }
}

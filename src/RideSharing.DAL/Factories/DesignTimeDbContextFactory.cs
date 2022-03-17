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
                Integrated Security = True; ");

            return new RideSharingDbContext(builder.Options);
        }
    }
}

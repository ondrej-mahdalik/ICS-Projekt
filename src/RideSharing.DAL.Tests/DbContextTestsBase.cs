using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RideSharing.Common.Tests;
using RideSharing.Common.Tests.Factories;
using Xunit;
using Xunit.Abstractions;

namespace RideSharing.DAL.Tests;

public class DbContextTestsBase : IAsyncLifetime
{
    protected DbContextTestsBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        // DbContextFactory = new DbContextTestingInMemoryFactory(GetType().Name, seedTestingData: true);
        DbContextFactory = new DbContextLocalDBTestingFactory(GetType().FullName!, true);
        // DbContextFactory = new DbContextSQLiteTestingFactory(GetType().FullName!, seedDALTestingData: true);

        RideSharingDbContextSUT = DbContextFactory.CreateDbContext();
    }

    protected IDbContextFactory<RideSharingDbContext> DbContextFactory { get; }
    protected RideSharingDbContext RideSharingDbContextSUT { get; }

    public async Task InitializeAsync()
    {
        await RideSharingDbContextSUT.Database.EnsureDeletedAsync();
        await RideSharingDbContextSUT.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await RideSharingDbContextSUT.Database.EnsureDeletedAsync();
        await RideSharingDbContextSUT.DisposeAsync();
    }
}

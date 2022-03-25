using System;
using System.Threading.Tasks;
using RideSharing.Common.Tests;
using RideSharing.Common.Tests.Factories;
using RideSharing.DAL.Factories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace RideSharing.DAL.Tests;

public class  DbContextTestsBase : IAsyncLifetime
{
    protected IDbContextFactory<RideSharingDbContext> DbContextFactory { get; }
    protected RideSharingDbContext RideSharingDbContextSUT { get; }

    protected DbContextTestsBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);
        
        // DbContextFactory = new DbContextTestingInMemoryFactory(GetType().Name, seedTestingData: true);
        // DbContextFactory = new DbContextLocalDBTestingFactory(GetType().FullName!, seedTestingData: true);
        DbContextFactory = new DbContextSQLiteTestingFactory(GetType().FullName!, seedTestingData: true);
        
        RideSharingDbContextSUT = DbContextFactory.CreateDbContext();
    }

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

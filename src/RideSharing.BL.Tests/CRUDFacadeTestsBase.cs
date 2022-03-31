using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.EntityFrameworkCore;
using RideSharing.Common.Tests;
using RideSharing.Common.Tests.Factories;
using RideSharing.DAL;
using RideSharing.DAL.UnitOfWork;
using Xunit;
using Xunit.Abstractions;

namespace RideSharing.BL.Tests;

public class CRUDFacadeTestsBase : IAsyncLifetime
{
    protected CRUDFacadeTestsBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        DbContextFactory = new DbContextSQLiteTestingFactory(GetType().FullName!, seedDALTestingData: true);

        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);

        var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(new[] {typeof(BusinessLogic),});
                cfg.AddCollectionMappers();

                using var dbContext = DbContextFactory.CreateDbContext();
                cfg.UseEntityFrameworkCoreModel<RideSharingDbContext>(dbContext.Model);
            }
        );
        Mapper = new Mapper(configuration);
        Mapper.ConfigurationProvider.AssertConfigurationIsValid();

    }

    protected IDbContextFactory<RideSharingDbContext> DbContextFactory { get; }
    protected Mapper Mapper { get; }

    protected UnitOfWorkFactory UnitOfWorkFactory { get; }

    public async Task InitializeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
        await dbx.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
    }
}

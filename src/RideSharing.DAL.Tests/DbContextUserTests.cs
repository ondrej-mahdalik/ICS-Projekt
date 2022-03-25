using System;
using System.Linq;
using System.Threading.Tasks;
using RideSharing.Common.Tests.Seeds;
using RideSharing.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using RideSharing.Common.Tests;
using Xunit;
using Xunit.Abstractions;

namespace RideSharing.DAL.Tests;

public class DbContextUserTests : DbContextTestsBase
{
    public DbContextUserTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task AddNew_User_Persisted()
    {
        //Arrange
        var entity = UserSeeds.EmptyUser with
        {
            Name = "Jonah",
            Surname = "Hanoi",
            Phone = "444555666",
            ImageUrl = "https://commons.wikimedia.org/wiki/File:Person_icon_BLACK-01.svg"
        };

        //Act
        RideSharingDbContextSUT.UserEntities.Add(entity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.UserEntities
            .SingleAsync(i => i.Id == entity.Id);

        DeepAssert.Equal(entity, actualEntity);

    }

    [Fact]
    public async Task GetById_Only_User()
    {
        //Arrange
            // seed - is already in DB

        //Act
        var entity = await RideSharingDbContextSUT.UserEntities
            .SingleAsync(i => i.Id == UserSeeds.ReservationUser.Id);

        //Assert
        DeepAssert.Equal(UserSeeds.ReservationUser with { Reservations = Array.Empty<ReservationEntity>() }, entity);
    }

    [Fact]
    public async Task GetById_IncludingVehicles_User()
    {
        //Act
        var entity = await RideSharingDbContextSUT.UserEntities
            .Include(i => i.Vehicles)
            .SingleAsync(i => i.Id == UserSeeds.DriverUser.Id);

        //Assert
        DeepAssert.Equal( UserSeeds.DriverUser, entity);
    }
}

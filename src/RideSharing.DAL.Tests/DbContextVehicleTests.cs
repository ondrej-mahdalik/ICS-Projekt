using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using RideSharing.Common.Tests.DALTestsSeeds;
using RideSharing.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using RideSharing.Common.Enums;
using RideSharing.Common.Tests;
using Xunit;
using Xunit.Abstractions;

namespace RideSharing.DAL.Tests;

public class DbContextVehicleTests : DbContextTestsBase
{
    public DbContextVehicleTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task AddNew_Vehicle()
    {
        //Arrange
        var entity = VehicleSeeds.EmptyVehicle with
        {
            OwnerId = UserSeeds.VehicleTestsUser.Id,
            VehicleType = VehicleType.Car,
            Make = "Make",
            Model = "Model",
            Registered = DateTime.Parse("05/16/1996", CultureInfo.InvariantCulture),
            Seats = 5,
            ImageUrl = "https://auta5p.eu/katalog/skoda/felicia_28.jpg"
        };

        //Act
        RideSharingDbContextSUT.VehicleEntities.Add(entity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.VehicleEntities.SingleAsync(i => i.Id == entity.Id);

        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task GetById_Only_Vehicle()
    {
        //Arrange
        var expected = VehicleSeeds.GetNoRelationsEntity(VehicleSeeds.Felicia);

        //Act
        var entity = await RideSharingDbContextSUT.VehicleEntities.SingleAsync(i => i.Id == VehicleSeeds.Felicia.Id);

        //Assert
        DeepAssert.Equal(expected, entity);
    }

    [Fact]
    public async Task GetById_IncludingOwner_Vehicle()
    {
        //Arrange
        var expected = VehicleSeeds.GetNoRelationsEntity(VehicleSeeds.Felicia) with
        {
            Owner = UserSeeds.GetNoRelationsEntity(UserSeeds.DriverUser)
        };

        //Act
        var entity = await RideSharingDbContextSUT.VehicleEntities.Include(i => i.Owner)
            .SingleAsync(i => i.Id == VehicleSeeds.Felicia.Id);

        //Assert
        DeepAssert.Equal(expected, entity, "Vehicles");
    }

    [Fact]
    public async Task GetById_IncludingRides_Vehicle()
    {
        //Arrange
        var expected = VehicleSeeds.GetNoRelationsEntity(VehicleSeeds.Felicia);
        expected.Rides.Add(RideSeeds.PragueBrno);
        expected.Rides.Add(RideSeeds.BrnoBratislava);

        //Act
        var entity = await RideSharingDbContextSUT.VehicleEntities.Include(i => i.Rides)
            .SingleAsync(i => i.Id == VehicleSeeds.Felicia.Id);

        //Assert
        DeepAssert.Equal(expected, entity, "Vehicle");
    }

    [Fact]
    public async Task Update_Vehicle()
    {
        //Arrange
        var baseEntity = VehicleSeeds.UpdateVehicle;
        var entity = baseEntity with
        {
            VehicleType = VehicleType.Car,
            Make = baseEntity.Make + "Update",
            Model = baseEntity.Make + "Update",
            Registered = DateTime.Parse("03/12/2005", CultureInfo.InvariantCulture),
            Seats = 15,
            ImageUrl = "new_image_path"
        };

        //Act
        RideSharingDbContextSUT.VehicleEntities.Update(entity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.VehicleEntities.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task Delete_Vehicle()
    {
        //Arrange
        var baseEntity = VehicleSeeds.DeleteVehicle;

        //Act
        RideSharingDbContextSUT.VehicleEntities.Remove(baseEntity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await RideSharingDbContextSUT.VehicleEntities.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task DeleteById_Vehicle()
    {
        //Arrange
        var baseEntity = VehicleSeeds.DeleteVehicle;

        //Act
        RideSharingDbContextSUT.VehicleEntities.Remove(
            RideSharingDbContextSUT.VehicleEntities.Single(i => i.Id == baseEntity.Id));
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await RideSharingDbContextSUT.VehicleEntities.AnyAsync(i => i.Id == baseEntity.Id));
    }
}

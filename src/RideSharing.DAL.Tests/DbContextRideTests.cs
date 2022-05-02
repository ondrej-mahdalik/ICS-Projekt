using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RideSharing.Common.Tests;
using RideSharing.Common.Tests.Seeds;
using Xunit;
using Xunit.Abstractions;

namespace RideSharing.DAL.Tests;

public class DbContextRideTests : DbContextTestsBase
{
    public DbContextRideTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task AddNew_Ride()
    {
        //Arrange
        var entity = RideSeeds.EmptyRideEntity with
        {
            SharedSeats = 4,
            FromName = "From",
            Distance = 200,
            ToName = "To",
            Departure = DateTime.Parse("02/22/2022 17:30", CultureInfo.InvariantCulture),
            Arrival = DateTime.Parse("02/22/2022 19:40", CultureInfo.InvariantCulture),
            VehicleId = VehicleSeeds.Felicia.Id,
            Note = "No eating in the car!"
        };

        //Act
        RideSharingDbContextSUT.RideEntities.Add(entity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.RideEntities.SingleAsync(i => i.Id == entity.Id);

        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task GetById_Only_Ride()
    {
        //Arrange
        var expected = RideSeeds.GetNoRelationsEntity(RideSeeds.PragueBrno);

        //Act
        var entity = await RideSharingDbContextSUT.RideEntities.SingleAsync(i => i.Id == RideSeeds.PragueBrno.Id);

        //Assert
        DeepAssert.Equal(expected, entity);
    }

    [Fact]
    public async Task GetById_IncludingVehicle_Ride()
    {
        //Arrange
        var expected = RideSeeds.GetNoRelationsEntity(RideSeeds.PragueBrno) with { Vehicle = VehicleSeeds.Felicia };

        //Act
        var entity = await RideSharingDbContextSUT.RideEntities.Include(i => i.Vehicle)
            .SingleAsync(i => i.Id == RideSeeds.PragueBrno.Id);

        //Assert
        DeepAssert.Equal(expected, entity, "Rides");
    }

    [Fact]
    public async Task GetById_IncludingReservations_Ride()
    {
        //Arrange
        var expected = RideSeeds.GetNoRelationsEntity(RideSeeds.PragueBrno);
        expected.Reservations.Add(ReservationSeeds.User1PragueBrno);
        expected.Reservations.Add(ReservationSeeds.User2PragueBrno);

        //Act
        var entity = await RideSharingDbContextSUT.RideEntities.Include(i => i.Reservations)
            .SingleAsync(i => i.Id == RideSeeds.PragueBrno.Id);

        //Assert
        DeepAssert.Equal(expected, entity, "Ride");
    }

    [Fact]
    public async Task GetById_IncludingReservationsUsers_Ride()
    {
        //Arrange
        var expected = RideSeeds.GetNoRelationsEntity(RideSeeds.PragueBrno);
        expected.Reservations.Add(ReservationSeeds.User1PragueBrno with { ReservingUser = UserSeeds.ReservationUser1 });
        expected.Reservations.Add(ReservationSeeds.User2PragueBrno with { ReservingUser = UserSeeds.ReservationUser2 });

        //Act
        var entity = await RideSharingDbContextSUT.RideEntities.Include(i => i.Reservations)
            .ThenInclude(i => i.ReservingUser)
            .SingleAsync(i => i.Id == RideSeeds.PragueBrno.Id);

        //Assert
        DeepAssert.Equal(expected, entity, "Ride", "Reservations");
    }

    [Fact]
    public async Task GetById_IncludingReviews_Ride()
    {
        //Arrange
        var expected = RideSeeds.GetNoRelationsEntity(RideSeeds.PragueBrno);
        expected.Reviews.Add(ReviewSeeds.DriverAuthoredPragueBrnoReview);
        expected.Reviews.Add(ReviewSeeds.DriverPragueBrnoReview);

        //Act
        var entity = await RideSharingDbContextSUT.RideEntities.Include(i => i.Reviews)
            .SingleAsync(i => i.Id == RideSeeds.PragueBrno.Id);

        //Assert
        DeepAssert.Equal(expected, entity, "Ride");
    }

    [Fact]
    public async Task Update_Ride()
    {
        //Arrange
        var baseEntity = RideSeeds.UpdateRide;
        var entity = baseEntity with
        {
            SharedSeats = 2,
            FromName = baseEntity.FromName + "update",
            Distance = baseEntity.Distance + 40,
            ToName = baseEntity.ToName + "update",
            Departure = DateTime.Parse("02/22/2020 17:30", CultureInfo.InvariantCulture),
            Arrival = DateTime.Parse("02/22/2020 19:40", CultureInfo.InvariantCulture),
            VehicleId = VehicleSeeds.Karosa.Id,
            Note = baseEntity.Note + "update"
        };

        //Act
        RideSharingDbContextSUT.RideEntities.Update(entity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.RideEntities.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task Delete_Ride()
    {
        //Arrange
        var baseEntity = RideSeeds.DeleteRide;

        //Act
        RideSharingDbContextSUT.RideEntities.Remove(baseEntity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await RideSharingDbContextSUT.RideEntities.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task DeleteById_Ride()
    {
        //Arrange
        var baseEntity = RideSeeds.DeleteRide;

        //Act
        RideSharingDbContextSUT.RideEntities.Remove(
            RideSharingDbContextSUT.RideEntities.Single(i => i.Id == baseEntity.Id));
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await RideSharingDbContextSUT.RideEntities.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task Delete_CascadeDeleteReservations_Ride()
    {
        //Arrange
        var baseEntity = RideSeeds.JustReservationRide;
        var baseEntityReservation = ReservationSeeds.JustOneRideReservation;

        Assert.True(await RideSharingDbContextSUT.ReservationEntities.AnyAsync(i => i.Id == baseEntityReservation.Id));

        //Act
        RideSharingDbContextSUT.RideEntities.Remove(baseEntity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await RideSharingDbContextSUT.VehicleEntities.AnyAsync(i => i.Id == baseEntityReservation.Id));
    }

    [Fact]
    public async Task Delete_CascadeDeleteReviews_Ride()
    {
        //Arrange
        var baseEntity = RideSeeds.JustReviewRide;
        var baseEntityReview = ReviewSeeds.JustRideReview;
        baseEntity.Reviews.Add(baseEntityReview);

        Assert.True(await RideSharingDbContextSUT.ReviewEntities.AnyAsync(i => i.Id == baseEntityReview.Id));

        //Act
        RideSharingDbContextSUT.RideEntities.Remove(baseEntity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await RideSharingDbContextSUT.ReviewEntities.AnyAsync(i => i.Id == baseEntityReview.Id));
    }
}

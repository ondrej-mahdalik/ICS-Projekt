using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using RideSharing.Common.Tests.DALTestsSeeds;
using RideSharing.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using RideSharing.Common.Tests;
using Xunit;
using Xunit.Abstractions;

namespace RideSharing.DAL.Tests;

public class DbContextReservationTests : DbContextTestsBase
{
    public DbContextReservationTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task AddNew_Reservation()
    {
        //Arrange
        var entity = ReservationSeeds.EmptyReservation with
        {
            ReservingUserId = UserSeeds.UserDelete.Id,
            RideId = RideSeeds.DeleteRide.Id,
            Seats = 2,
            Timestamp = DateTime.Parse("02/20/2022 12:20", CultureInfo.InvariantCulture)
        };

        //Act
        RideSharingDbContextSUT.ReservationEntities.Add(entity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.ReservationEntities
            .SingleAsync(i => i.Id == entity.Id);

        DeepAssert.Equal(entity, actualEntity);

    }

    [Fact]
    public async Task GetById_Reservation()
    {
        //Arrange
        var expected = ReservationSeeds.GetNoRelationsEntity(ReservationSeeds.User1PragueBrno);

        //Act
        var entity = await RideSharingDbContextSUT.ReservationEntities
            .SingleAsync(i => i.Id == ReservationSeeds.User1PragueBrno.Id);

        //Assert
        DeepAssert.Equal(expected, entity);
    }

    [Fact]
    public async Task GetById_IncludingUser_Reservation()
    {
        //Arrange
        var expected = ReservationSeeds.GetNoRelationsEntity(ReservationSeeds.User1PragueBrno) with {ReservingUser = UserSeeds.ReservationUser1};

        //Act
        var entity = await RideSharingDbContextSUT.ReservationEntities
            .Include(i => i.ReservingUser)
            .SingleAsync(i => i.Id == ReservationSeeds.User1PragueBrno.Id);

        //Assert
        DeepAssert.Equal(expected, entity, "Reservations");
    }

    [Fact]
    public async Task GetById_IncludingUserIncludingReview_Reservation()
    {
        //Arrange
        var user = UserSeeds.GetNoRelationsEntity(UserSeeds.ReservationUser1);
        user.Reviews.Add(ReviewSeeds.DriverAuthoredPragueBrnoReview);
        user.Reviews.Add(ReviewSeeds.CascadeDeleteSubmittedReview);
        var expected = ReservationSeeds.GetNoRelationsEntity(ReservationSeeds.User1PragueBrno) with { ReservingUser = user };

        //Act
        var entity = await RideSharingDbContextSUT.ReservationEntities
            .Include(i => i.ReservingUser)
            .ThenInclude(i => i!.Reviews)
            .SingleAsync(i => i.Id == ReservationSeeds.User1PragueBrno.Id);

        //Assert
        DeepAssert.Equal(expected, entity, "Reservations", "ReviewedUser");
    }

    [Fact]
    public async Task GetById_IncludingRide_Reservation()
    {
        //Arrange
        var expected = ReservationSeeds.GetNoRelationsEntity(ReservationSeeds.User1PragueBrno) with { Ride = RideSeeds.PragueBrno };

        //Act
        var entity = await RideSharingDbContextSUT.ReservationEntities
            .Include(i => i.Ride)
            .SingleAsync(i => i.Id == ReservationSeeds.User1PragueBrno.Id);

        //Assert
        DeepAssert.Equal(expected, entity, "Reservations");
    }

    [Fact]
    public async Task Update_Reservation()
    {
        //Arrange
        var baseEntity = ReservationSeeds.UpdateReservation;
        var entity =
            baseEntity with
            {
                ReservingUserId = UserSeeds.ReservationUser2.Id,
                RideId = RideSeeds.BrnoBratislava.Id,
                Seats = 2,
                Timestamp = DateTime.Parse("04/20/2022 11:10", CultureInfo.InvariantCulture)
            };

        //Act
        RideSharingDbContextSUT.ReservationEntities.Update(entity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.ReservationEntities.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task Delete_Reservation()
    {
        //Arrange
        var baseEntity = ReservationSeeds.DeleteReservation;

        //Act
        RideSharingDbContextSUT.ReservationEntities.Remove(baseEntity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await RideSharingDbContextSUT.ReservationEntities.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task DeleteById_Reservation()
    {
        //Arrange
        var baseEntity = ReservationSeeds.DeleteReservation;


        //Act
        RideSharingDbContextSUT.ReservationEntities.Remove(
            RideSharingDbContextSUT.ReservationEntities.Single(i => i.Id == baseEntity.Id));
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await RideSharingDbContextSUT.ReservationEntities.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task Delete_NoCascadeUserDelete_Reservation()
    {
        //Arrange
        var baseEntity = ReservationSeeds.CascadeDeleteReservation;
        var reservingUser = UserSeeds.CascadeDeleteUser;


        //Act
        RideSharingDbContextSUT.ReservationEntities.Remove(baseEntity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.True(await RideSharingDbContextSUT.UserEntities.AnyAsync(i => i.Id == reservingUser.Id));
    }

    [Fact]
    public async Task Delete_NoCascadeRideDelete_Reservation()
    {
        //Arrange
        var baseEntity = ReservationSeeds.CascadeDeleteReservation;
        var ride = RideSeeds.CascadeDeleteRide;


        //Act
        RideSharingDbContextSUT.ReservationEntities.Remove(baseEntity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.True(await RideSharingDbContextSUT.RideEntities.AnyAsync(i => i.Id == ride.Id));
    }
}

﻿using System;
using System.Collections.Generic;
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

public class DbContextUserTests : DbContextTestsBase
{
    public DbContextUserTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task AddNew_User()
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
            var expected = UserSeeds.GetNoRelationsEntity(UserSeeds.ReservationUser1);

        //Act
        var entity = await RideSharingDbContextSUT.UserEntities
            .SingleAsync(i => i.Id == UserSeeds.ReservationUser1.Id);

        //Assert
        DeepAssert.Equal(expected, entity);
    }

    [Fact]
    public async Task GetById_IncludingVehicles_User()
    {
        //Arrange
        var expected = UserSeeds.GetNoRelationsEntity(UserSeeds.DriverUser);
        expected.Vehicles.Add(VehicleSeeds.Felicia);
        expected.Vehicles.Add(VehicleSeeds.Karosa);

        //Act
        var entity = await RideSharingDbContextSUT.UserEntities
            .Include(i => i.Vehicles)
            .SingleAsync(i => i.Id == UserSeeds.DriverUser.Id);

        //Assert
        DeepAssert.Equal(expected, entity, "Owner");
    }

    [Fact]
    public async Task GetById_IncludingVehiclesIncludingRides_User()
    {
        //Arrange
        var feliciaWRides = VehicleSeeds.GetNoRelationsEntity(VehicleSeeds.Felicia);
        feliciaWRides.Rides.Add(RideSeeds.BrnoBratislava);
        feliciaWRides.Rides.Add(RideSeeds.PragueBrno);
        var expected = UserSeeds.GetNoRelationsEntity(UserSeeds.DriverUser);
        expected.Vehicles.Add(feliciaWRides);
        expected.Vehicles.Add(VehicleSeeds.Karosa);
        
        //Act
        var entity = await RideSharingDbContextSUT.UserEntities
            .Include(i => i.Vehicles)
            .ThenInclude(i => i.Rides)
            .SingleAsync(i => i.Id == UserSeeds.DriverUser.Id);

        //Assert
        DeepAssert.Equal(expected, entity, "Owner", "Vehicle");
    }

    [Fact]
    public async Task GetById_IncludingReservations_User()
    {
        //Arrange
        var expected = UserSeeds.GetNoRelationsEntity(UserSeeds.ReservationUser1);
        expected.Reservations.Add(ReservationSeeds.User1PragueBrno); 
        expected.Reservations.Add(ReservationSeeds.User1BrnoBratislava);

        //Act
        var entity = await RideSharingDbContextSUT.UserEntities
            .Include(i => i.Reservations)
            .SingleAsync(i => i.Id == UserSeeds.ReservationUser1.Id);

        //Assert
        DeepAssert.Equal(expected, entity, "ReservingUser");
    }

    [Fact]
    public async Task GetById_IncludingReviews_User()
    {
        //Arrange
        var expected = UserSeeds.GetNoRelationsEntity(UserSeeds.DriverUser);
        expected.Reviews.Add(ReviewSeeds.DriverPragueBrnoReview);

        //Act
        var entity = await RideSharingDbContextSUT.UserEntities
            .Include(i => i.Reviews)
            .SingleAsync(i => i.Id == UserSeeds.DriverUser.Id);

        //Assert
        DeepAssert.Equal(expected, entity, "ReviewedUser");
    }

    [Fact]
    public async Task GetById_IncludingSubmittedReviews_User()
    {
        //Arrange
        var expected = UserSeeds.GetNoRelationsEntity(UserSeeds.DriverUser);
        expected.SubmittedReviews.Add(ReviewSeeds.DriverAuthoredPragueBrnoReview);

        //Act
        var entity = await RideSharingDbContextSUT.UserEntities
            .Include(i => i.SubmittedReviews)
            .SingleAsync(i => i.Id == UserSeeds.DriverUser.Id);

        //Assert
        DeepAssert.Equal(expected, entity, "AuthorUser");
    }

    [Fact]
    public async Task Update_User()
    {
        //Arrange
        var baseEntity = UserSeeds.UserUpdate;
        var entity =
            baseEntity with
            {
                Name = baseEntity.Name + "Updated",
                Surname = baseEntity.Surname + "Updated",
                Phone = baseEntity.Phone + "Updated",
                ImageUrl = baseEntity.ImageUrl + "Updated"
            }; 

        //Act
        RideSharingDbContextSUT.UserEntities.Update(entity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.UserEntities.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task Delete_User()
    {
        //Arrange
        var baseEntity = UserSeeds.UserDelete;
        
        //Act
        RideSharingDbContextSUT.UserEntities.Remove(baseEntity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await RideSharingDbContextSUT.UserEntities.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task DeleteById_User()
    {
        //Arrange
        var baseEntity = UserSeeds.UserDelete;


        //Act
        RideSharingDbContextSUT.UserEntities.Remove(
            RideSharingDbContextSUT.UserEntities.Single(i => i.Id == baseEntity.Id));
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await RideSharingDbContextSUT.UserEntities.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task Delete_CascadeDeleteVehicles_User()
    {
        //Arrange
        var baseEntity = UserSeeds.CascadeDeleteUser;
        var baseEntityVehicle = VehicleSeeds.CascadeDeleteVehicle;

        //Act
        RideSharingDbContextSUT.UserEntities.Remove(baseEntity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await RideSharingDbContextSUT.VehicleEntities.AnyAsync(i => i.Id == baseEntityVehicle.Id));

    }
    
    [Fact]
    public async Task Delete_CascadeDeleteReservations_User()
    {
        //Arrange
        var baseEntity = UserSeeds.CascadeDeleteUser;
        var baseEntityOwnReservation = ReservationSeeds.CascadeDeleteReservation;

        //Act
        RideSharingDbContextSUT.UserEntities.Remove(baseEntity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await RideSharingDbContextSUT.ReservationEntities.AnyAsync(i => i.Id == baseEntityOwnReservation.Id));
    }

    [Fact]
    public async Task Delete_CascadeDeleteObtainedReviews_User()
    {
        //Arrange
        var baseEntity = UserSeeds.CascadeDeleteUser;
        var baseEntityObtainedReview = ReviewSeeds.CascadeDeleteObtainedReview;

        //Act
        RideSharingDbContextSUT.UserEntities.Remove(baseEntity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await RideSharingDbContextSUT.ReviewEntities.AnyAsync(i => i.Id == baseEntityObtainedReview.Id));
    }

    [Fact]
    public async Task Delete_CascadeDeleteSubmittedReviews_User()
    {
        //Arrange
        var baseEntity = UserSeeds.CascadeDeleteUser;
        var baseEntitySubmittedReview = ReviewSeeds.CascadeDeleteSubmittedReview;

        //Act
        RideSharingDbContextSUT.UserEntities.Remove(baseEntity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await RideSharingDbContextSUT.ReviewEntities.AnyAsync(i => i.Id == baseEntitySubmittedReview.Id));
    }
    
}

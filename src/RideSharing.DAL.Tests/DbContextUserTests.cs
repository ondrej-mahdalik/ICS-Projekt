using System;
using System.Collections.Generic;
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
            .SingleAsync(i => i.Id == UserSeeds.ReservationUser1.Id);

        //Assert
        DeepAssert.Equal(UserSeeds.ReservationUser1 with { Reservations = Array.Empty<ReservationEntity>() }, entity);
    }

    [Fact]
    public async Task GetById_IncludingVehicles_User()
    {
        //Act
        var entity = await RideSharingDbContextSUT.UserEntities
            .Include(i => i.Vehicles)
            .SingleAsync(i => i.Id == UserSeeds.VehiclesUser.Id);

        //Assert
        DeepAssert.Equal(UserSeeds.VehiclesUser, entity);
    }

    [Fact]
    public async Task GetById_IncludingCreatedRides_User()
    {
        //Act
        var entity = await RideSharingDbContextSUT.UserEntities
            .Include(i => i.CreatedRides)
            .SingleAsync(i => i.Id == UserSeeds.CreatedRidesUser.Id);

        //Assert
        DeepAssert.Equal(UserSeeds.CreatedRidesUser, entity);
    }

    [Fact]
    public async Task GetById_IncludingCreatedRides_User()
    {
        //Act
        var entity = await RideSharingDbContextSUT.UserEntities
            .Include(i => i.CreatedRides)
            .SingleAsync(i => i.Id == UserSeeds.CreatedRidesUser.Id);

        //Assert
        DeepAssert.Equal(UserSeeds.CreatedRidesUser, entity);
    }

    [Fact]
    public async Task Update_User_Persisted()
    {
        //Arrange
        var baseEntity = UserSeeds.UserEntityUpdate;
        var entity =
            baseEntity with
            {
                Name = baseEntity.Name + "Updated",
                Description = baseEntity.Description + "Updated",
                Duration = default,
                FoodType = FoodType.None,
                ImageUrl = baseEntity.ImageUrl + "Updated",
            };

        //Act
        RideSharingDbContextSUT.UserEntities.Update(entity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Users.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    //[Fact]
    //public async Task Delete_IngredientAmount_Deleted()
    //{
    //    //Arrange
    //    var baseEntity = UserSeeds.UserEntityDelete;

    //    //Act
    //    CookBookDbContextSUT.Users.Remove(baseEntity);
    //    await CookBookDbContextSUT.SaveChangesAsync();

    //    //Assert
    //    Assert.False(await CookBookDbContextSUT.Users.AnyAsync(i => i.Id == baseEntity.Id));
    //}

    //[Fact]
    //public async Task DeleteById_IngredientAmount_Deleted()
    //{
    //    //Arrange
    //    var baseEntity = UserSeeds.UserEntityDelete;

    //    //Act
    //    CookBookDbContextSUT.Remove(
    //        CookBookDbContextSUT.Users.Single(i => i.Id == baseEntity.Id));
    //    await CookBookDbContextSUT.SaveChangesAsync();

    //    //Assert
    //    Assert.False(await CookBookDbContextSUT.Users.AnyAsync(i => i.Id == baseEntity.Id));
    //}
}

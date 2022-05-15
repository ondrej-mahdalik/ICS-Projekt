using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;
using RideSharing.Common.Tests;
using RideSharing.Common.Tests.Seeds;
using Xunit;
using Xunit.Abstractions;

namespace RideSharing.BL.Tests;

public sealed class UserFacadeTests : CRUDFacadeTestsBase
{
    private readonly UserFacade _userFacadeSUT;

    public UserFacadeTests(ITestOutputHelper output) : base(output)
    {
        _userFacadeSUT = new UserFacade(UnitOfWorkFactory, Mapper);
    }

    [Fact]
    public async Task Create_WithNonExistingItem_DoesNotThrow()
    {
        var user = new UserDetailModel(
            "Jan",
            "Václavík",
            "746652914"
        );

        await _userFacadeSUT.SaveAsync(user);
    }

    [Fact]
    public async Task GetAll_Single_SeededUser()
    {
        var users = await _userFacadeSUT.GetAsync();
        var user = users.Single(i => i.Id == UserSeeds.JustSubmittedReviewUser.Id);
        DeepAssert.Equal(Mapper.Map<UserListModel>(UserSeeds.JustSubmittedReviewUser), user);
    }

    [Fact]
    public async Task GetById_SeededUser()
    {
        var user = await _userFacadeSUT.GetAsync(UserSeeds.JustSubmittedReviewUser.Id);
        DeepAssert.Equal(Mapper.Map<UserDetailModel>(UserSeeds.JustSubmittedReviewUser), user, "Vehicles",
            "ReceivedReviews", "SubmittedReviews");
        Assert.Empty(user!.Vehicles);
    }

    [Fact]
    public async Task GetById_NonExistent()
    {
        var user = await _userFacadeSUT.GetAsync(
            Guid.Parse("D2453E4A-2A52-4199-A8BE-254893C575B6")); // Guid for non-existing seed
        Assert.Null(user);
    }

    [Fact]
    public async Task SeededUserWithRide_DeleteById_DoesNotThrow()
    {
        await _userFacadeSUT.DeleteAsync(UserSeeds.DriverUser.Id);
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.UserEntities.AnyAsync(i => i.Id == UserSeeds.DriverUser.Id));
    }

    [Fact]
    public async Task SeededUserWithoutRide_DeleteById_DoesNotThrow()
    {
        await _userFacadeSUT.DeleteAsync(UserSeeds.JustVehicleOwnerUser.Id);
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.UserEntities.AnyAsync(i => i.Id == UserSeeds.JustVehicleOwnerUser.Id));
        Assert.Equal(0,
            await dbxAssert.VehicleEntities.CountAsync(i => i.OwnerId == UserSeeds.JustVehicleOwnerUser.Id));
    }

    [Fact]
    public async Task NewUser_InsertOrUpdate_UserAdded()
    {
        //Arrange
        var user = new UserDetailModel(
            "Jan",
            "Václavík",
            "745541942"
        );

        //Act
        user = await _userFacadeSUT.SaveAsync(user);

        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var userFromDb = await dbxAssert.UserEntities.SingleAsync(i => i.Id == user.Id);
        DeepAssert.Equal(user, Mapper.Map<UserDetailModel>(userFromDb));
    }

    [Fact]
    public async Task SeededUser_InsertOrUpdate_UserUpdated()
    {
        //Arrange
        var user = new UserDetailModel
        (
            UserSeeds.DriverUser.Name,
            UserSeeds.DriverUser.Surname,
            UserSeeds.DriverUser.Phone
        ) { Id = UserSeeds.DriverUser.Id };
        user.Name += "updated";
        user.Surname += "updated";

        //Act
        await _userFacadeSUT.SaveAsync(user);

        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var userFromDb = await dbxAssert.UserEntities.SingleAsync(i => i.Id == user.Id);
        var updatedUser = Mapper.Map<UserDetailModel>(userFromDb);
        DeepAssert.Equal(user, updatedUser);
    }

    [Fact]
    public async Task SeededUser_Delete_DeletesAllReviewsHeObtained()
    {
        await _userFacadeSUT.DeleteAsync(UserSeeds.JustObtainedReviewUser.Id);
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.Equal(0,
            await dbxAssert.ReviewEntities.CountAsync(i =>
                i.Ride!.Vehicle!.OwnerId == UserSeeds.JustObtainedReviewUser.Id));
    }

    [Fact]
    public async Task SeededUser_Delete_KeepsAllReviewsHeSubmitted()
    {
        // Arrange
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var originalCount = await dbxAssert.ReviewEntities.CountAsync(i => i.AuthorUserId == null);

        // Act
        await _userFacadeSUT.DeleteAsync(UserSeeds.JustSubmittedReviewUser.Id);

        // Assert
        var newCount = await dbxAssert.ReviewEntities.CountAsync(i => i.AuthorUserId == null);
        Assert.Equal(1, newCount - originalCount);
    }
}

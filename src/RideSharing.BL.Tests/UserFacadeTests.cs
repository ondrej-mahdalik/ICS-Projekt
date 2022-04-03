using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using RideSharing.BL.Models;
using RideSharing.BL.Facades;
using Microsoft.EntityFrameworkCore;
using RideSharing.Common.Tests.DALTestsSeeds;
using RideSharing.Common.Tests;
using Xunit.Abstractions;
using RideSharing.DAL.Entities;
using System.Collections.Generic;

namespace RideSharing.BL.Tests
{
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
                Name: @"Jan",
                Surname: @"Václavík",
                Phone: @"746652914"
            );

            var _ = await _userFacadeSUT.SaveAsync(user);
        }

        [Fact]
        public async Task GetAll_Single_SeededUser()
        {
            var users = await _userFacadeSUT.GetAsync();
               var user = users.Single(i => i.Id == UserSeeds.DriverUser.Id);
            var user2 = Mapper.Map<UserListModel>(UserSeeds.DriverUser);
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            int vehicleCount = await dbxAssert.VehicleEntities.CountAsync(i => i.OwnerId == UserSeeds.DriverUser.Id);
            var reviewsOnUser = await dbxAssert.ReviewEntities.Where(i => i.ReviewedUserId == UserSeeds.DriverUser.Id).ToListAsync();
            List<ReviewDetailModel> reviewsOnUserDetail = new List<ReviewDetailModel>();
            foreach (ReviewEntity review in reviewsOnUser)
            {
                reviewsOnUserDetail.Add(Mapper.Map<ReviewDetailModel>(review));
            }
            int upcomingRidesCount = await dbxAssert.ReservationEntities.CountAsync(i => i.ReservingUserId == UserSeeds.DriverUser.Id && i.Ride.Departure > DateTime.Now);
            upcomingRidesCount += await dbxAssert.RideEntities.CountAsync(i => i.Vehicle.OwnerId == UserSeeds.DriverUser.Id && i.Departure > DateTime.Now);
            DeepAssert.Equal(user2, user, new string[] {"NumberOfVehicles", "Reviews", "UpcomingRidesCount"});
            Assert.Equal(vehicleCount, user.NumberOfVehicles);
            //Assert.Equal<ReviewDetailModel>(reviewsOnUserDetail, user.Reviews);
            Assert.Equal(user.UpcomingRidesCount, upcomingRidesCount);
        }

        [Fact]
        public async Task GetById_SeededUser()
        {
            var user = await _userFacadeSUT.GetAsync(UserSeeds.DriverUser.Id);
            DeepAssert.Equal(Mapper.Map<UserDetailModel>(UserSeeds.DriverUser), user);
        }

        [Fact]
        public async Task GetById_NonExistent()
        {
            var user = await _userFacadeSUT.GetAsync(Guid.Parse("D2453E4A-2A52-4199-A8BE-254893C575B6")); // Random Guid, Empty seed is used in Cookbook
            Assert.Null(user);
        }

        // Not sure if delete should fail
        [Fact]
        public async Task SeededUser_DeleteByIdDeleted()
        {
           await Assert.ThrowsAsync<DbUpdateException>(async () => await _userFacadeSUT.DeleteAsync(UserSeeds.DriverUser.Id));
        }

        [Fact]
        public async Task NewUser_InsertOrUpdate_UserAdded()
        {
            //Arrange
            var user = new UserDetailModel(
                Name: "Jan",
                Surname: "Václavík",
                Phone: "745541942"
            );

            //Act
            user = await _userFacadeSUT.SaveAsync(user);

            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var userFromDb = await dbxAssert.UserEntities.SingleAsync(i => i.Id == user.Id);
           // DeepAssert.Equal(user, Mapper.Map<UserDetailModel>(userFromDb));
            DeepAssert.Equal(user, Mapper.Map<UserDetailModel>(userFromDb));
        }
        [Fact]
        public async Task SeededUser_InsertOrUpdate_UserUpdated()
        {
            //Arrange
            var user = new UserDetailModel
            (
                Name: UserSeeds.DriverUser.Name,
                Surname: UserSeeds.DriverUser.Surname,
                Phone: UserSeeds.DriverUser.Phone
            )
            {
                Id = UserSeeds.DriverUser.Id
            };
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
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using RideSharing.BL.Models;
using RideSharing.BL.Facades;
using Microsoft.EntityFrameworkCore;
using RideSharing.Common.Tests.Seeds;
using Xunit.Abstractions;

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
                var user = users.Single(i => i.Id == UserSeeds.JohnDoe.Id);
                //bool selectionOk = Mapper.Map<UserDetailModel>(UserSeeds.JohnDoe) == user
            Assert.Equal(Mapper.Map<UserListModel>(UserSeeds.JohnDoe), user);
        }

        [Fact]
        public async Task GetById_SeededUser()
        {
            var user = await _userFacadeSUT.GetAsync(UserSeeds.ElonTusk.Id);
            Assert.Equal(Mapper.Map<UserDetailModel>(UserSeeds.ElonTusk), user);
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
           await Assert.ThrowsAsync<DbUpdateException>(async () => await _userFacadeSUT.DeleteAsync(UserSeeds.JohnDoe.Id));
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
            Assert.Equal(user, Mapper.Map<UserDetailModel>(userFromDb));
        }
        [Fact]
        public async Task SeededUser_InsertOrUpdate_UserUpdated()
        {
            //Arrange
            var user = new UserDetailModel
            (
                Name: UserSeeds.JohnDoe.Name,
                Surname: UserSeeds.JohnDoe.Surname,
                Phone: UserSeeds.JohnDoe.Phone
            )
            {
                Id = UserSeeds.JohnDoe.Id
            };
            user.Name += "updated";
            user.Surname += "updated";

            //Act
            await _userFacadeSUT.SaveAsync(user);

            //Assert
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var userFromDb = await dbxAssert.UserEntities.SingleAsync(i => i.Id == user.Id);
            var updatedUser = Mapper.Map<UserDetailModel>(userFromDb);
            //Assert.Equal(user.Id, updatedUser.Id);
            //Assert.Equal(user.Name, updatedUser.Name);
            //Assert.Equal(user.Surname, updatedUser.Surname);
            Assert.Same(userFromDb, updatedUser);
            //Assert.Equal(user.Id, Mapper.Map<UserDetailModel>(userFromDb).Id);
           // DeepAssert.Equal(user, Mapper.Map<UserDetailModel>(ingredientFromDb));
        }
    }
}

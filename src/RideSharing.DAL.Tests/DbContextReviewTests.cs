using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RideSharing.Common.Tests;
using RideSharing.Common.Tests.Seeds;
using Xunit;
using Xunit.Abstractions;

namespace RideSharing.DAL.Tests;

public class DbContextReviewTests : DbContextTestsBase
{
    public DbContextReviewTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task AddNew_Review()
    {
        //Arrange
        var entity = ReviewSeeds.EmptyReviewEntity with
        {
            RideId = RideSeeds.BrnoBratislava.Id, AuthorUserId = UserSeeds.ReservationUser2.Id, Rating = 1
        };

        //Act
        RideSharingDbContextSUT.ReviewEntities.Add(entity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.ReviewEntities.SingleAsync(i => i.Id == entity.Id);

        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task GetById_Only_Review()
    {
        //Arrange
        var expected = ReviewSeeds.GetNoRelationsEntity(ReviewSeeds.DriverPragueBrnoReview);

        //Act
        var entity =
            await RideSharingDbContextSUT.ReviewEntities.SingleAsync(i =>
                i.Id == ReviewSeeds.DriverPragueBrnoReview.Id);

        //Assert
        DeepAssert.Equal(expected, entity);
    }

    [Fact]
    public async Task GetById_IncludingAuthorUser_Review()
    {
        //Arrange
        var expected = ReviewSeeds.GetNoRelationsEntity(ReviewSeeds.DriverPragueBrnoReview) with
        {
            AuthorUser = UserSeeds.GetNoRelationsEntity(UserSeeds.ReservationUser1)
        };

        //Act
        var entity = await RideSharingDbContextSUT.ReviewEntities.Include(i => i.AuthorUser)
            .SingleAsync(i => i.Id == ReviewSeeds.DriverPragueBrnoReview.Id);

        //Assert
        DeepAssert.Equal(expected, entity, "SubmittedReviews");
    }

    [Fact]
    public async Task GetById_IncludingRide_Review()
    {
        //Arrange
        var expected = ReviewSeeds.GetNoRelationsEntity(ReviewSeeds.DriverPragueBrnoReview) with
        {
            Ride = RideSeeds.GetNoRelationsEntity(RideSeeds.PragueBrno)
        };

        //Act
        var entity = await RideSharingDbContextSUT.ReviewEntities.Include(i => i.Ride)
            .SingleAsync(i => i.Id == ReviewSeeds.DriverPragueBrnoReview.Id);

        //Assert
        DeepAssert.Equal(expected, entity, "Reviews");
    }

    [Fact]
    public async Task Delete_Review()
    {
        //Arrange
        var baseEntity = ReviewSeeds.DeleteReview;

        //Act
        RideSharingDbContextSUT.ReviewEntities.Remove(baseEntity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await RideSharingDbContextSUT.ReviewEntities.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task DeleteById_Review()
    {
        //Arrange
        var baseEntity = ReviewSeeds.DeleteReview;

        //Act
        RideSharingDbContextSUT.ReviewEntities.Remove(
            RideSharingDbContextSUT.ReviewEntities.Single(i => i.Id == baseEntity.Id));
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await RideSharingDbContextSUT.ReviewEntities.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task Delete_NoCascadeAuthorUserDelete_Reservation()
    {
        //Arrange
        var baseEntity = ReviewSeeds.DeleteReview;
        var authorUser = UserSeeds.CascadeDeleteUser;

        //Act
        RideSharingDbContextSUT.ReviewEntities.Remove(baseEntity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.True(await RideSharingDbContextSUT.UserEntities.AnyAsync(i => i.Id == authorUser.Id));
    }

    [Fact]
    public async Task Delete_NoCascadeReviewedUserDelete_Reservation()
    {
        //Arrange
        var baseEntity = ReviewSeeds.DeleteReview;
        var reservingUser = UserSeeds.ReservationUser2;

        //Act
        RideSharingDbContextSUT.ReviewEntities.Remove(baseEntity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.True(await RideSharingDbContextSUT.UserEntities.AnyAsync(i => i.Id == reservingUser.Id));
    }

    [Fact]
    public async Task Delete_NoCascadeRideDelete_Reservation()
    {
        //Arrange
        var baseEntity = ReviewSeeds.DeleteReview;
        var ride = RideSeeds.CascadeDeleteRide;

        //Act
        RideSharingDbContextSUT.ReviewEntities.Remove(baseEntity);
        await RideSharingDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.True(await RideSharingDbContextSUT.RideEntities.AnyAsync(i => i.Id == ride.Id));
    }
}

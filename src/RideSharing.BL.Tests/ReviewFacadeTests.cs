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

public sealed class ReviewFacadeTests : CRUDFacadeTestsBase
{
    private readonly ReviewFacade _reviewFacadeSUT;

    public ReviewFacadeTests(ITestOutputHelper output) : base(output)
    {
        _reviewFacadeSUT = new ReviewFacade(UnitOfWorkFactory, Mapper);
    }

    [Fact]
    public async Task Create_WithNonExistingItem_DoesNotThrow()
    {
        // Arrange
        var review = new ReviewDetailModel(
            5
        );

        // Act
        var _ = await _reviewFacadeSUT.SaveAsync(review);
    }

    [Fact]
    public async Task GetAll_Single_SeededReview()
    {
        var reviews = await _reviewFacadeSUT.GetAsync();
        var review = reviews.Single(i => i.Id == ReviewSeeds.DriverPragueBrnoReview.Id);
        Assert.Equal(Mapper.Map<ReviewDetailModel>(ReviewSeeds.DriverPragueBrnoReview).Id, review.Id);
    }

    [Fact]
    public async Task GetById_SeededReview()
    {
        var review = await _reviewFacadeSUT.GetAsync(ReviewSeeds.DriverPragueBrnoReview.Id);
        Assert.NotNull(review);
        Assert.Equal(Mapper.Map<ReviewDetailModel>(ReviewSeeds.DriverPragueBrnoReview).Id, review!.Id);
    }

    [Fact]
    public async Task GetById_NonExistent()
    {
        var review = await _reviewFacadeSUT.GetAsync(Guid.Parse("D2453E4A-2A52-4199-A8BE-254853C575B6")); // Guid for non-existing seed
        Assert.Null(review);
    }

    [Fact]
    public async Task SeededReview_DeleteByIdDeleted()
    {
        await _reviewFacadeSUT.DeleteAsync(ReviewSeeds.DriverPragueBrnoReview.Id);
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.ReviewEntities.AnyAsync(i => i.Id == ReviewSeeds.DriverPragueBrnoReview.Id));
    }

    [Fact]
    public async Task NewReview_InsertOrUpdate_ReviewAdded()
    {
        var review = new ReviewDetailModel(
            3
        )
        {
            AuthorUser = new UserListModel(
                UserSeeds.ReservationUser1.Name,
                UserSeeds.ReservationUser1.Surname,
                UserSeeds.ReservationUser1.Phone,
                UserSeeds.ReservationUser1.ImageUrl
            ) { Id = UserSeeds.ReservationUser1.Id },
            Ride = new RideListModel(
                RideSeeds.JustReviewRide.FromName,
                RideSeeds.JustReviewRide.ToName,
                Distance: RideSeeds.JustReviewRide.Distance,
                SharedSeats: RideSeeds.JustReviewRide.SharedSeats,
                Departure: RideSeeds.JustReviewRide.Departure,
                Arrival: RideSeeds.JustReviewRide.Arrival
            ) { Id = RideSeeds.JustReviewRide.Id }
        };
        review = await _reviewFacadeSUT.SaveAsync(review);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var reviewFromDb = await dbxAssert.ReviewEntities.SingleAsync(i => i.Id == review.Id);
        DeepAssert.Equal(review, Mapper.Map<ReviewDetailModel>(reviewFromDb), "AuthorUser", "Ride");
        DeepAssert.Equal(review.AuthorUser, Mapper.Map<UserListModel>(UserSeeds.ReservationUser1));
        DeepAssert.Equal(review.Ride, Mapper.Map<RideListModel>(RideSeeds.JustReviewRide), "Vehicle");
    }

    [Fact]
    public async Task NewReview_InsertOrUpdate_ReviewUpdated()
    {
        var review = new ReviewDetailModel(
            ReviewSeeds.JustRideReview.Rating
        )
        {
            Id = ReviewSeeds.JustRideReview.Id,
            AuthorUser = new UserListModel(
                UserSeeds.ReservationUser1.Name,
                UserSeeds.ReservationUser1.Surname,
                UserSeeds.ReservationUser1.Phone,
                UserSeeds.ReservationUser1.ImageUrl
            ) { Id = UserSeeds.ReservationUser1.Id },
            Ride = new RideListModel(
                RideSeeds.JustReviewRide.FromName,
                RideSeeds.JustReviewRide.ToName,
                Distance: RideSeeds.JustReviewRide.Distance,
                SharedSeats: RideSeeds.JustReviewRide.SharedSeats,
                Departure: RideSeeds.JustReviewRide.Departure,
                Arrival: RideSeeds.JustReviewRide.Arrival
            ) { Id = RideSeeds.JustReviewRide.Id }
        };
        review.Rating = 2;
        await _reviewFacadeSUT.SaveAsync(review);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var reviewFromDb = await dbxAssert.ReviewEntities.SingleAsync(i => i.Id == review.Id);
        var updatedReview = Mapper.Map<ReviewDetailModel>(reviewFromDb);
        DeepAssert.Equal(review, updatedReview, "AuthorUser", "Ride");
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using RideSharing.BL.Models;
using RideSharing.DAL.Entities;
using RideSharing.BL.Facades;
using Microsoft.EntityFrameworkCore;
using RideSharing.Common.Tests.DALTestsSeeds;
using Xunit.Abstractions;

namespace RideSharing.BL.Tests
{
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
            var review = new ReviewDetailModel(
                Rating: 5

            ){
              ReviewedUser = Mapper.Map<UserDetailModel>(new UserEntity(
                  Id: Guid.Parse("6A855B23-307F-4B31-B91B-9350D0E0BEDA"),
                  Name: "Harrison",
                  Surname: "Ford",
                  Phone: "464546431",
                  ImageUrl: null
                  )), 
              AuthorUser = Mapper.Map<UserDetailModel>(new UserEntity(
                  Id: Guid.Parse("6E855B23-307F-4B31-B91B-9350D0E0BEDA"),
                  Name: "Johnny",
                  Surname: "Valda",
                  Phone: "464513431",
                  ImageUrl: null
                  )),
              Ride = Mapper.Map<RideDetailModel>(new RideEntity(
                    Id: Guid.Parse("7C855B23-307F-4B31-B91B-9350D0E0BEDA"),
                    FromName: "Olomouc",
                    SharedSeats: 4,
                    FromLatitude: 4.25,
                    FromLongitude: 5.46,
                    Distance: 25,
                    ToName: "Brno",
                    ToLatitude: 25.5,
                    ToLongitude: 4654.48,
                    Departure: DateTime.Now,
                    Arrival: DateTime.Now.AddSeconds(30),
                    VehicleId: VehicleSeeds.Felicia.Id,
                    Note: null
                  ))

            };


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
            var review = await _reviewFacadeSUT.GetAsync(Guid.Parse("D2453E4A-2A52-4199-A8BE-254853C575B6")); // Random Guid, Empty seed is used in Cookbook
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
                Rating: 3
            )
            { ReviewedUser = Mapper.Map<UserDetailModel>(UserSeeds.DriverUser),
              Ride = Mapper.Map<RideDetailModel>(RideSeeds.JustReviewRide),
              AuthorUser = Mapper.Map<UserDetailModel>(UserSeeds.ReservationUser1)
            };
            review = await _reviewFacadeSUT.SaveAsync(review);

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var reviewFromDb = await dbxAssert.ReviewEntities.SingleAsync(i => i.Id == review.Id);
            Assert.Equal(review.Id, reviewFromDb.Id);
        }
        [Fact]
        public async Task NewReview_InsertOrUpdate_ReviewUpdated()
        {
            var review = new ReviewDetailModel(
                Rating: ReviewSeeds.DriverPragueBrnoReview.Rating
            )
            {
                Id = ReviewSeeds.DriverPragueBrnoReview.Id
            };
            review.Rating = 2;
            await _reviewFacadeSUT.SaveAsync(review);

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var reviewFromDb = await dbxAssert.UserEntities.SingleAsync(i => i.Id == review.Id);
            var updatedReview = Mapper.Map<ReviewDetailModel>(reviewFromDb);
            Assert.Equal(review.Rating, updatedReview.Rating);
        }
    }
}

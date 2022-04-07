using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using RideSharing.BL.Models;
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
            // Arrange
            var review = new ReviewDetailModel(
                Rating: 5

            ) {
                ReviewedUser = new UserDetailModel(
                  Name: "Harrison",
                  Surname: "Ford",
                  Phone: "464546431"
                ),
                AuthorUser = new UserDetailModel(
                  Name: "Johnny",
                  Surname: "Valda",
                  Phone: "464513431"
                ),
                Ride = new RideDetailModel(
                    FromName: "Olomouc",
                    FromLatitude: 4.25,
                    FromLongitude: 5.46,
                    ToName: "Brno",
                    ToLatitude: 25.5,
                    ToLongitude: 4654.48,
                    Distance: 25,
                    SharedSeats: 4,
                    Departure: DateTime.Now,
                    Arrival: DateTime.Now.AddMinutes(30)
                  )
                {
                    Vehicle = new VehicleDetailModel(
                     OwnerId: VehicleSeeds.Karosa.OwnerId,
                     VehicleType: VehicleSeeds.Karosa.VehicleType,
                     Make: VehicleSeeds.Karosa.Make,
                     Model: VehicleSeeds.Karosa.Model,
                     Registered: VehicleSeeds.Karosa.Registered,
                     Seats: VehicleSeeds.Karosa.Seats
                     )
              }
              
            };

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
            { 
                ReviewedUser = new UserDetailModel(
                    Name: UserSeeds.DriverUser.Name,
                    Surname:UserSeeds.DriverUser.Surname,
                    Phone: UserSeeds.DriverUser.Phone
                ),
                AuthorUser = new UserDetailModel(
                  Name: UserSeeds.ReservationUser1.Name,
                  Surname: UserSeeds.ReservationUser1.Surname,
                  Phone: UserSeeds.ReservationUser1.Phone
                ), 
                Ride = new RideDetailModel(
                    FromName: RideSeeds.JustReviewRide.FromName,
                    FromLatitude: RideSeeds.JustReviewRide.FromLatitude,
                    FromLongitude: RideSeeds.JustReviewRide.FromLongitude,
                    ToName: RideSeeds.JustReviewRide.ToName,
                    ToLatitude: RideSeeds.JustReviewRide.ToLatitude,
                    ToLongitude: RideSeeds.JustReviewRide.ToLongitude,
                    Distance: RideSeeds.JustReviewRide.Distance,
                    SharedSeats: RideSeeds.JustReviewRide.SharedSeats,
                    Departure: RideSeeds.JustReviewRide.Departure,
                    Arrival: RideSeeds.JustReviewRide.Arrival
                )
                {
                    Vehicle = new VehicleDetailModel(
                        OwnerId: UserSeeds.DriverUser.Id,
                        VehicleType: VehicleSeeds.Felicia.VehicleType,
                        Make: VehicleSeeds.Felicia.Make,
                        Model: VehicleSeeds.Felicia.Model,
                        Registered: VehicleSeeds.Felicia.Registered,
                        Seats: VehicleSeeds.Felicia.Seats
                    )
                }
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
                Rating: 3
            )
            {
                //Id = ReviewSeeds.JustRideReview.Id,
                ReviewedUser = new UserDetailModel(
                    Name: UserSeeds.DriverUser.Name,
                    Surname: UserSeeds.DriverUser.Surname,
                    Phone: UserSeeds.DriverUser.Phone,
                    ImageUrl:UserSeeds.DriverUser.ImageUrl
                ),
                AuthorUser = new UserDetailModel(
                    Name: UserSeeds.ReservationUser1.Name,
                    Surname: UserSeeds.ReservationUser1.Surname,
                    Phone: UserSeeds.ReservationUser1.Phone,
                    ImageUrl: UserSeeds.ReservationUser1.ImageUrl
                ),
                Ride = new RideDetailModel(
                    FromName: RideSeeds.JustReviewRide.FromName,
                    FromLatitude: RideSeeds.JustReviewRide.FromLatitude,
                    FromLongitude: RideSeeds.JustReviewRide.FromLongitude,
                    ToName: RideSeeds.JustReviewRide.ToName,
                    ToLatitude: RideSeeds.JustReviewRide.ToLatitude,
                    ToLongitude: RideSeeds.JustReviewRide.ToLongitude,
                    Distance: RideSeeds.JustReviewRide.Distance,
                    SharedSeats: RideSeeds.JustReviewRide.SharedSeats,
                    Departure: RideSeeds.JustReviewRide.Departure,
                    Arrival: RideSeeds.JustReviewRide.Arrival,
                    Note: RideSeeds.JustReviewRide.Note
                )
                {
                    Vehicle = new VehicleDetailModel(
                        OwnerId: UserSeeds.DriverUser.Id,
                        VehicleType: VehicleSeeds.Felicia.VehicleType,
                        Make: VehicleSeeds.Felicia.Make,
                        Model: VehicleSeeds.Felicia.Model,
                        Registered: VehicleSeeds.Felicia.Registered,
                        Seats: VehicleSeeds.Felicia.Seats,
                        ImageUrl: VehicleSeeds.Felicia.ImageUrl
                    )
                }
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

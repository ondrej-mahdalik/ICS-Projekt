using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;
using RideSharing.Common.Tests.Seeds;
using Xunit;
using Xunit.Abstractions;

namespace RideSharing.BL.Tests;

public sealed class ReservationFacadeTests : CRUDFacadeTestsBase
{
    private readonly ReservationFacade _reservationFacadeSUT;

    public ReservationFacadeTests(ITestOutputHelper output) : base(output)
    {
        _reservationFacadeSUT = new ReservationFacade(UnitOfWorkFactory, Mapper);
    }

    [Fact]
    public async Task CreateReservation()
    {
        // Arrange
        ReservationDetailModel reservation = new(DateTime.Now, 2)
        {
            ReservingUser = Mapper.Map<UserDetailModel>(UserSeeds.JustSubmittedReviewUser),
            Ride = Mapper.Map<RideDetailModel>(RideSeeds.JustReviewRide)
        };

        // Act
        await _reservationFacadeSUT.SaveAsync(reservation);

        // Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
       var temp = await dbxAssert.ReservationEntities.Include(x => x.ReservingUser).Include(x => x.Ride).FirstOrDefaultAsync(x =>
            x.ReservingUserId == UserSeeds.JustSubmittedReviewUser.Id && x.RideId == RideSeeds.JustReviewRide.Id);
       Assert.NotNull(temp);
    }
}

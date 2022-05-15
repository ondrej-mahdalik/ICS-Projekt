using System;
using System.Linq;
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
        ReservationDetailModel reservation =
            new(DateTime.Now, 2, UserSeeds.JustSubmittedReviewUser.Id, RideSeeds.JustReviewRide.Id);

        // Act
        await _reservationFacadeSUT.SaveAsync(reservation);

        // Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var temp = await dbxAssert.ReservationEntities.Include(x => x.ReservingUser).Include(x => x.Ride).FirstOrDefaultAsync(x => 
            x.ReservingUserId == UserSeeds.JustSubmittedReviewUser.Id && x.RideId == RideSeeds.JustReviewRide.Id);
        Assert.NotNull(temp);
    }
    
    [Fact]
    public async Task UpdateReservation()
    {
        // Arrange
        ReservationDetailModel reservation =
            new(DateTime.Now, 2, UserSeeds.ReservationUser1.Id, RideSeeds.PragueBrno.Id);

        reservation.ReservingUser = Mapper.Map<UserDetailModel>(UserSeeds.ReservationUser2);

        // Act
        await _reservationFacadeSUT.SaveAsync(reservation);

        // Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var temp = await dbxAssert.ReservationEntities.Include(x => x.ReservingUser).Include(x => x.Ride).FirstOrDefaultAsync(x => 
            x.ReservingUserId == UserSeeds.ReservationUser2.Id && x.RideId == RideSeeds.PragueBrno.Id);
        Assert.NotNull(temp);
    }
    
    [Fact]
    public async Task GetAll_Single_SeededReservation()
    {
        var reservations = await _reservationFacadeSUT.GetAsync();
        var reservation = reservations.Single(i => i.Id == ReservationSeeds.DriverBrnoBratislava.Id);
        Assert.Equal(Mapper.Map<ReservationDetailModel>(ReservationSeeds.DriverBrnoBratislava).Id, reservation.Id);
        Assert.Equal(Mapper.Map<UserDetailModel>(UserSeeds.DriverUser).Id, reservation.ReservingUser!.Id);
        Assert.Equal(Mapper.Map<RideDetailModel>(RideSeeds.BrnoBratislava).Id, reservation.Ride!.Id);
    }
    
    [Fact]
    public async Task GetById_SeededReservation()
    {
        var reservation = await _reservationFacadeSUT.GetAsync(ReservationSeeds.User1BrnoBratislava.Id);
        Assert.NotNull(reservation);
        Assert.Equal(Mapper.Map<ReservationDetailModel>(ReservationSeeds.User1BrnoBratislava).Id, reservation!.Id);
    }
    
    [Fact]
    public async Task SeededReservation_DeleteByIdDeleted()
    {
        await _reservationFacadeSUT.DeleteAsync(ReservationSeeds.DriverBrnoBratislava.Id);
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.ReservationEntities.AnyAsync(i => i.Id == ReservationSeeds.DriverBrnoBratislava.Id));
    }
    
    [Fact]
    public async Task GetById_NonExistentReservation()
    {
        var reservation = await _reservationFacadeSUT.GetAsync(Guid.Parse("D2453E4A-2A52-4199-A8BE-254853C575B6")); // Guid for non-existing seed
        Assert.Null(reservation);
    }
}

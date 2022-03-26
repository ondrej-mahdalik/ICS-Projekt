using RideSharing.BL.Facades;
using Xunit.Abstractions;
using RideSharing.BL.Models;
using System;
using RideSharing.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;
using System.Threading.Tasks;

namespace RideSharing.BL.Tests;

public sealed class RideFacadeTests : CRUDFacadeTestsBase
{
    private readonly RideFacade _rideFacadeSUT;

    public RideFacadeTests(ITestOutputHelper output) : base(output)
    {
        _rideFacadeSUT = new RideFacade(UnitOfWorkFactory, Mapper);
    }

    // Error in mapping Vehicle type
    [Fact]
    public async Task Create_WithNonExistingItem_DoesNotThrow()
    {
        var ride = new RideDetailModel(
            FromName: "Praha",
            FromLatitude: 50.073658,
            FromLongitude: 14.418540,
            ToName: "Brno",
            ToLatitude: 49.195061,
            ToLongitude: 16.606836,
            Distance: 203,
            SharedSeats: 4,
            Departure: new DateTime(2022, 3, 25, 09, 04, 00),
            Arrival: new DateTime(2022, 3, 25, 12, 04, 00)
        );
        var _ = await _rideFacadeSUT.SaveAsync(ride);
    }

    [Fact]
    public async Task GetAll_Single_SeededRide()
    {
        var rides = await _rideFacadeSUT.GetAsync();
            var ride = rides.Single(i => i.Id == RideSeeds.PrahaBrno.Id);
        Assert.Equal(Mapper.Map<RideListModel>(RideSeeds.PrahaBrno).Id, ride.Id);
    }

    [Fact]
    public async Task GetById__SeededRide()
    {
        var ride = await _rideFacadeSUT.GetAsync(RideSeeds.PrahaBrno.Id);
        Assert.Equal(Mapper.Map<RideDetailModel>(RideSeeds.PrahaBrno).Id, ride?.Id);
    }

    [Fact]
    public async Task GetById_NonExistent()
    {
        var ride = await _rideFacadeSUT.GetAsync(Guid.Parse("D2453C4A-2A52-4199-A8BE-254893C575B6")); // Random Guid, Empty seed is used in Cookbook
        Assert.Null(ride);
    }
    [Fact]
    public async Task SeededRide_DeleteByIdDeleted()
    {
        await Assert.ThrowsAsync<DbUpdateException>(async () => await _rideFacadeSUT.DeleteAsync(RideSeeds.PrahaBrno.Id));
    }
    [Fact]
    public async Task NewRide_InsertOrUpdate_RideAdded()
    {
        var ride = new RideDetailModel(
            FromName: "Wien",
            FromLatitude: 48.20849,
            FromLongitude: 16.37208,
            ToName: "Milan",
            ToLatitude: 45.464664,
            ToLongitude: 9.188540,
            Distance: 861,
            SharedSeats: 3,
            Departure: new DateTime(2022, 03, 26, 11, 40, 00),
            Arrival: new DateTime(2022, 03, 26, 19, 41, 00)
        );
        ride = await _rideFacadeSUT.SaveAsync(ride);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var rideFromDb = await dbxAssert.RideEntities.SingleAsync(i => i.Id == ride.Id);
        Assert.Equal(ride.Id, ride.Id);
    }
    [Fact]
    public async Task NewRide_InsertOrUpdate_RideUpdated()
    {
        var ride = new RideDetailModel(
            FromName: RideSeeds.PrahaBrno.FromName,
            FromLatitude: RideSeeds.PrahaBrno.FromLatitude,
            FromLongitude: RideSeeds.PrahaBrno.FromLongitude,
            ToName: RideSeeds.PrahaBrno.ToName,
            ToLatitude: RideSeeds.PrahaBrno.ToLatitude,
            ToLongitude: RideSeeds.PrahaBrno.ToLongitude,
            Distance: RideSeeds.PrahaBrno.Distance,
            Departure: RideSeeds.PrahaBrno.Departure,
            Arrival: RideSeeds.PrahaBrno.Arrival,
            SharedSeats:RideSeeds.PrahaBrno.SharedSeats
        )
        {
            Id = RideSeeds.PrahaBrno.Id
        };
        ride.ToName = "Ostrava";
        ride.ToLatitude = 49.820923;
        ride.ToLongitude = 18.262524;
        await _rideFacadeSUT.SaveAsync(ride);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var rideFromDb = await dbxAssert.RideEntities.SingleAsync(i => i.Id == ride.Id);
        var updatedRide = Mapper.Map<RideDetailModel>(rideFromDb);
        Assert.Equal(ride.ToName, updatedRide.ToName);
        Assert.Equal(ride.ToLatitude, updatedRide.ToLatitude);
        Assert.Equal(ride.ToLongitude, updatedRide.ToLongitude);
    }

}

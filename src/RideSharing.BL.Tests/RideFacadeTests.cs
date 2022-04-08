using RideSharing.BL.Facades;
using Xunit.Abstractions;
using RideSharing.BL.Models;
using System;
using RideSharing.Common.Tests.DALTestsSeeds;
using Microsoft.EntityFrameworkCore;
using Xunit;
using RideSharing.DAL.Entities;
using System.Linq;
using RideSharing.Common.Tests;
using System.Threading.Tasks;

namespace RideSharing.BL.Tests;

public sealed class RideFacadeTests : CRUDFacadeTestsBase
{
    private readonly RideFacade _rideFacadeSUT;

    public RideFacadeTests(ITestOutputHelper output) : base(output)
    {
        _rideFacadeSUT = new RideFacade(UnitOfWorkFactory, Mapper);
    }

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
        )
        {
            Vehicle = new VehicleDetailModel(
                 OwnerId: VehicleSeeds.Felicia.OwnerId,
                 VehicleType: VehicleSeeds.Felicia.VehicleType,
                 Make: VehicleSeeds.Felicia.Make,
                 Model: VehicleSeeds.Felicia.Model,
                 Registered: VehicleSeeds.Felicia.Registered,
                 Seats: VehicleSeeds.Felicia.Seats
           )
        };
        var _ = await _rideFacadeSUT.SaveAsync(ride);
    }

    [Fact]
    public async Task GetAll_Single_SeededRide()
    {
        var rides = await _rideFacadeSUT.GetAsync();
            var ride = rides.Single(i => i.Id == RideSeeds.BrnoBratislava.Id);
        DeepAssert.Equal(Mapper.Map<RideListModel>(RideSeeds.BrnoBratislava), ride, new string[] {"Driver", "Vehicle"});
        DeepAssert.Equal(ride.Vehicle, Mapper.Map<RideListVehicleModel>(VehicleSeeds.Felicia));
    }

    [Fact]
    public async Task GetById__SeededRide()
    {
        var ride = await _rideFacadeSUT.GetAsync(RideSeeds.BrnoBratislava.Id);
        DeepAssert.Equal(Mapper.Map<RideDetailModel>(RideSeeds.BrnoBratislava), ride,
            new string[] { "Driver", "Vehicle", "Reservations"});
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
        var vehicle = _rideFacadeSUT.DeleteAsync(RideSeeds.BrnoBratislava.Id);
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.RideEntities.AnyAsync(i => i.Id == RideSeeds.BrnoBratislava.Id));
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
        )
        {
            Vehicle = new VehicleDetailModel(
                 OwnerId: VehicleSeeds.Felicia.OwnerId,
                 VehicleType: VehicleSeeds.Felicia.VehicleType,
                 Make: VehicleSeeds.Felicia.Make,
                 Model: VehicleSeeds.Felicia.Model,
                 Registered: VehicleSeeds.Felicia.Registered,
                 Seats: VehicleSeeds.Felicia.Seats
           )
        };
        ride = await _rideFacadeSUT.SaveAsync(ride);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var rideFromDb = await dbxAssert.RideEntities.SingleAsync(i => i.Id == ride.Id);
        Assert.Equal(ride.Id, ride.Id);
    }
    [Fact]
    public async Task NewRide_InsertOrUpdate_RideUpdated()
    {
        // Arrange
        var ride = new RideDetailModel(
            FromName: RideSeeds.BrnoBratislava.FromName,
            FromLatitude: RideSeeds.BrnoBratislava.FromLatitude,
            FromLongitude: RideSeeds.BrnoBratislava.FromLongitude,
            ToName: RideSeeds.BrnoBratislava.ToName,
            ToLatitude: RideSeeds.BrnoBratislava.ToLatitude,
            ToLongitude: RideSeeds.BrnoBratislava.ToLongitude,
            Distance: RideSeeds.BrnoBratislava.Distance,
            Departure: RideSeeds.BrnoBratislava.Departure,
            Arrival: RideSeeds.BrnoBratislava.Arrival,
            SharedSeats:RideSeeds.BrnoBratislava.SharedSeats
        )
        {
            Id = RideSeeds.BrnoBratislava.Id,
            Vehicle = new VehicleDetailModel(
                 OwnerId: VehicleSeeds.Karosa.OwnerId,
                 VehicleType: VehicleSeeds.Karosa.VehicleType,
                 Make: VehicleSeeds.Karosa.Make,
                 Model: VehicleSeeds.Karosa.Model,
                 Registered: VehicleSeeds.Karosa.Registered,
                 Seats: VehicleSeeds.Karosa.Seats
           )

        };

        // Act
        ride.ToName = "Ostrava";
        ride.ToLatitude = 49.820923;
        ride.ToLongitude = 18.262524;
        await _rideFacadeSUT.SaveAsync(ride);

        // Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var rideFromDb = await dbxAssert.RideEntities.SingleAsync(i => i.Id == ride.Id);
        var updatedRide = Mapper.Map<RideDetailModel>(rideFromDb);
        DeepAssert.Equal(ride, updatedRide);
    }

    [Fact]
    public async Task DeleteRide_KeepsAllItsReviews()
    {
        await _rideFacadeSUT.DeleteAsync(RideSeeds.PragueBrno.Id);
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.True(await dbxAssert.ReviewEntities.CountAsync(i => i.RideId == null) == 2);
    }
}

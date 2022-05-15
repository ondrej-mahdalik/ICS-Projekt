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
            "Praha",
            "Brno",
            203,
            4,
            new DateTime(2022, 3, 25, 09, 04, 00),
            new DateTime(2022, 3, 25, 12, 04, 00),
            VehicleSeeds.Felicia.Id
        );
        var _ = await _rideFacadeSUT.SaveAsync(ride);
    }

    [Fact]
    public async Task GetAll_Single_SeededRide()
    {
        var rides = await _rideFacadeSUT.GetAsync();
        var ride = rides.Single(i => i.Id == RideSeeds.BrnoBratislava.Id);
        DeepAssert.Equal(Mapper.Map<RideRecentListModel>(RideSeeds.BrnoBratislava), ride, "Driver", "Vehicle");
    }

    [Fact]
    public async Task GetById__SeededRide()
    {
        var ride = await _rideFacadeSUT.GetAsync(RideSeeds.BrnoBratislava.Id);
        DeepAssert.Equal(Mapper.Map<RideDetailModel>(RideSeeds.BrnoBratislava), ride, "Driver", "Vehicle",
            "Reservations");
    }

    [Fact]
    public async Task GetById_NonExistent()
    {
        var ride = await _rideFacadeSUT.GetAsync(Guid.Parse("D2453C4A-2A52-4199-A8BE-254893C575B6")); // Guid for non-existing seed
        Assert.Null(ride);
    }

    [Fact]
    public async Task SeededRide_DeleteByIdDeleted()
    {
        await _rideFacadeSUT.DeleteAsync(RideSeeds.BrnoBratislava.Id);
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.RideEntities.AnyAsync(i => i.Id == RideSeeds.BrnoBratislava.Id));
    }

    [Fact]
    public async Task NewRide_InsertOrUpdate_RideAdded()
    {
        var ride = new RideDetailModel(
            "Wien",
            "Milan",
            861,
            3,
            new DateTime(2022, 03, 26, 11, 40, 00),
            new DateTime(2022, 03, 26, 19, 41, 00),
            VehicleSeeds.Felicia.Id
        );

        ride = await _rideFacadeSUT.SaveAsync(ride);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var rideFromDb = await dbxAssert.RideEntities.SingleAsync(i => i.Id == ride.Id);
        DeepAssert.Equal(ride, Mapper.Map<RideDetailModel>(rideFromDb), "Reservations", "Vehicle");
        DeepAssert.Equal(ride.Vehicle, Mapper.Map<VehicleListModel>(VehicleSeeds.Felicia));
    }

    [Fact]
    public async Task NewRide_InsertOrUpdate_RideUpdated()
    {
        // Arrange
        var ride = new RideDetailModel(
            RideSeeds.BrnoBratislava.FromName,
            RideSeeds.BrnoBratislava.ToName,
            RideSeeds.BrnoBratislava.Distance,
            Departure: RideSeeds.BrnoBratislava.Departure,
            Arrival: RideSeeds.BrnoBratislava.Arrival,
            SharedSeats: RideSeeds.BrnoBratislava.SharedSeats,
            VehicleId: VehicleSeeds.Karosa.Id
        )
        {
            Id = RideSeeds.BrnoBratislava.Id,
        };

        // Act
        ride.ToName = "Ostrava";
        await _rideFacadeSUT.SaveAsync(ride);

        // Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var rideFromDb = await dbxAssert.RideEntities.Include(entity => entity.Vehicle)
            .SingleAsync(i => i.Id == ride.Id);
        var updatedRide = Mapper.Map<RideDetailModel>(rideFromDb);
        DeepAssert.Equal(ride, updatedRide);
    }
}

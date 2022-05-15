using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;
using RideSharing.Common.Enums;
using RideSharing.Common.Tests;
using RideSharing.Common.Tests.Seeds;
using Xunit;
using Xunit.Abstractions;

namespace RideSharing.BL.Tests;

public sealed class VehicleFacadeTests : CRUDFacadeTestsBase
{
    private readonly VehicleFacade _vehicleFacadeSUT;

    public VehicleFacadeTests(ITestOutputHelper output) : base(output)
    {
        _vehicleFacadeSUT = new VehicleFacade(UnitOfWorkFactory, Mapper);
    }

    [Fact]
    public async Task Create_WithNonExistingItem_DoesNotThrow()
    {
        var vehicle = new VehicleDetailModel(
            UserSeeds.DriverUser.Id,
            VehicleType.Car,
            "Ferrari",
            "250 gto",
            new DateTime(2010, 10, 25),
            4
        );
        var _ = await _vehicleFacadeSUT.SaveAsync(vehicle);
    }

    [Fact]
    public async Task GetAll_Single_SeededVehicle()
    {
        var vehicles = await _vehicleFacadeSUT.GetAsync();
        var vehicle = vehicles.Single(i => i.Id == VehicleSeeds.Felicia.Id);
        DeepAssert.Equal(vehicle, Mapper.Map<VehicleListModel>(VehicleSeeds.Felicia));
    }

    [Fact]
    public async Task GetById_SeededVehicle()
    {
        var vehicle = await _vehicleFacadeSUT.GetAsync(VehicleSeeds.Felicia.Id);
        Assert.Equal(Mapper.Map<VehicleDetailModel>(VehicleSeeds.Felicia).Id, vehicle?.Id);
    }

    [Fact]
    public async Task GetById_NonExistent()
    {
        var vehicle =
            await _vehicleFacadeSUT.GetAsync(Guid.Parse("D2453E4A-2A52-4199-A8BF-254893C575B6")); // Guid for non-existing seed
        Assert.Null(vehicle);
    }

    [Fact]
    public async Task GetVehiclesByOwnerId()
    {
        var vehicles = await _vehicleFacadeSUT.GetByOwnerAsync(UserSeeds.DriverUser.Id);
        Assert.Equal(2, vehicles.Count());
    }

    [Fact]
    public async Task SeededVehicleWithoutRide_DeleteById_DoesNotThrow()
    {
        var vehicle = _vehicleFacadeSUT.DeleteAsync(VehicleSeeds.Karosa.Id);
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.VehicleEntities.AnyAsync(i => i.Id == VehicleSeeds.Karosa.Id));
    }

    [Fact]
    public async Task SeededVehicleWithRide_DeleteById_DoesNotThrow()
    {
        await _vehicleFacadeSUT.DeleteAsync(VehicleSeeds.Felicia.Id);
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.VehicleEntities.AnyAsync(i => i.Id == VehicleSeeds.Felicia.Id));
    }

    [Fact]
    public async Task NewVehicle_InsertOrUpdate_VehicleAdded()
    {
        var vehicle = new VehicleDetailModel(
            UserSeeds.DriverUser.Id,
            VehicleType.Car,
            "Volvo",
            "V90",
            new DateTime(2018, 5, 5),
            4
        );
        vehicle = await _vehicleFacadeSUT.SaveAsync(vehicle);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var vehicleFromDb = await dbxAssert.VehicleEntities.SingleAsync(i => i.Id == vehicle.Id);
      //  DeepAssert.Equal(vehicle, Mapper.Map<VehicleDetailModel>(vehicleFromDb));
        Assert.Equal(vehicle.Id, vehicleFromDb.Id);
    }

    [Fact]
    public async Task NewVehicle_InsertOrUpdate_VehicleUpdated()
    {
        var vehicle = new VehicleDetailModel(
            VehicleSeeds.Felicia.OwnerId,
            VehicleSeeds.Felicia.VehicleType,
            VehicleSeeds.Felicia.Make,
            VehicleSeeds.Felicia.Model,
            VehicleSeeds.Felicia.Registered,
            VehicleSeeds.Felicia.Seats
        ) { Id = VehicleSeeds.Felicia.Id };
        vehicle.Make = "Trabant";
        await _vehicleFacadeSUT.SaveAsync(vehicle);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var vehicleFromDb = await dbxAssert.VehicleEntities.SingleAsync(i => i.Id == vehicle.Id);
        var updatedVehicle = Mapper.Map<VehicleDetailModel>(vehicleFromDb);
        DeepAssert.Equal(vehicle, updatedVehicle);
    }
}

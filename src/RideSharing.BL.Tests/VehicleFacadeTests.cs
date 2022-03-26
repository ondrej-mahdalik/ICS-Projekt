﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RideSharing.BL.Facades;
using RideSharing.BL.Models;
using RideSharing.Common.Tests.Seeds;
using Xunit;
using Xunit.Abstractions;
using RideSharing.Common.Enums;

namespace RideSharing.BL.Tests
{
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
                OwnerId: UserSeeds.JohnDoe.Id,
                VehicleType: VehicleType.Car,
                Make: "Ferrari",
                Model: "250 gto",
                Registered: new DateTime(2010, 10, 25),
                Seats: 4
            );
            var _ = await _vehicleFacadeSUT.SaveAsync(vehicle);
        }

        [Fact]
        public async Task GetAll_Single_SeededVehicle()
        {
            var vehicles = await _vehicleFacadeSUT.GetAsync();
                var vehicle = vehicles.Single(i => i.Id == VehicleSeeds.Felicia.Id);
            Assert.Equal(vehicle.Id, Mapper.Map<VehicleListModel>(VehicleSeeds.Felicia).Id);
        }

        [Fact]
        public async Task GetById_SeededVehicle()
        {
            var vehicle = await _vehicleFacadeSUT.GetAsync(VehicleSeeds.Felicia.Id);
            Assert.Equal(Mapper.Map<VehicleDetailModel>(VehicleSeeds.Felicia).Id, vehicle.Id);
        }

        [Fact]
        public async Task GetById_NonExistent()
        {
            var vehicle =
                await _vehicleFacadeSUT.GetAsync(Guid.Parse("D2453E4A-2A52-4199-A8BF-254893C575B6")); // Random Guid, Empty seed is used in Cookbook
            Assert.Null(vehicle);
        }

        [Fact]
        public async Task SeededVehicle_DeleteByIdDeleted()
        {
            var vehicle = _vehicleFacadeSUT.DeleteAsync(VehicleSeeds.Felicia.Id);
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            Assert.False(await dbxAssert.VehicleEntities.AnyAsync(i => i.Id == VehicleSeeds.Felicia.Id));

        }
        [Fact]
        public async Task NewReview_InsertOrUpdate_VehicleAdded()
        {

            var vehicle = new VehicleDetailModel(
                OwnerId: UserSeeds.ElonTusk.Id,
                VehicleType: VehicleType.Car,
                Make: "Volvo",
                Model: "V90",
                Registered: new DateTime(2018, 5, 5),
                Seats: 4
            );
                vehicle = await _vehicleFacadeSUT.SaveAsync(vehicle);
            
            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var vehicleFromDb = await dbxAssert.VehicleEntities.SingleAsync(i => i.Id == vehicle.Id);
            Assert.Equal(vehicle.Id, vehicleFromDb.Id);
        }
        [Fact]
        public async Task NewReview_InsertOrUpdate_VehicleUpdated()
        {
            var vehicle = new VehicleDetailModel(
                OwnerId: VehicleSeeds.Felicia.OwnerId,
                VehicleType: VehicleSeeds.Felicia.VehicleType,
                Make: VehicleSeeds.Felicia.Make,
                Model: VehicleSeeds.Felicia.Model,
                Registered: VehicleSeeds.Felicia.Registered,
                Seats: VehicleSeeds.Felicia.Seats
            )
            {
                Id = VehicleSeeds.Felicia.Id
            };
            vehicle.Make = "Trabant";
            await _vehicleFacadeSUT.SaveAsync(vehicle);

            await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
            var vehicleFromDb = await dbxAssert.VehicleEntities.SingleAsync(i => i.Id == vehicle.Id);
            var updatedVehicle = Mapper.Map<VehicleDetailModel>(vehicleFromDb);
            Assert.Equal(vehicle.Make, updatedVehicle.Make);
        }
    }
}

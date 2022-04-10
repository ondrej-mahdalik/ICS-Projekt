using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;
using RideSharing.Common.Enums;

namespace RideSharing.Common.Tests.Seeds;

public static class VehicleSeeds
{
    public static readonly VehicleEntity EmptyVehicle = new(
        Id: default,
        OwnerId: default,
        VehicleType: default,
        Make: string.Empty,
        Model: string.Empty,
        Registered: default,
        Seats: default,
        ImageUrl: default
    );

    public static readonly VehicleEntity Felicia = new(
        Id: Guid.Parse(input: "0a5ef2a1-d541-45ac-82a3-6b63a24d0572"),
        OwnerId: UserSeeds.DriverUser.Id,
        VehicleType: VehicleType.Car,
        Make: "Škoda",
        Model: "Felicia",
        Registered: DateTime.Parse("05/16/1996",
            CultureInfo.InvariantCulture),
        Seats: 5,
        ImageUrl: "https://auta5p.eu/katalog/skoda/felicia_28.jpg"
    );

    public static readonly VehicleEntity Karosa = new(
        Id: Guid.Parse(input: "14CD044E-F183-47BD-9A4C-68050E9111E7"),
        OwnerId: UserSeeds.DriverUser.Id,
        VehicleType: VehicleType.Bus,
        Make: "Karosa",
        Model: "LC 757",
        Registered: DateTime.Parse("03/12/1983",
            CultureInfo.InvariantCulture),
        Seats: 20,
        ImageUrl: "https://upload.wikimedia.org/wikipedia/commons/c/c6/Bus_LC757-HD12_Brno.jpg"
    );

    public static readonly VehicleEntity CascadeDeleteVehicle = Karosa with
    {
        Id = Guid.Parse("8BA11DD7-4062-4A5A-B896-82A093CCD0E3"),
        OwnerId = UserSeeds.CascadeDeleteUser.Id
    };

    public static readonly VehicleEntity UpdateVehicle = Karosa with
    {
        Id = Guid.Parse("74c5234d-a52a-48f8-8e26-a461b40650e6"),
        OwnerId = UserSeeds.VehicleTestsUser.Id
    };

    public static readonly VehicleEntity DeleteVehicle = UpdateVehicle with
    {
        Id = Guid.Parse("02dc5f5b-dbae-464e-bfa4-44d7e87b6ae9")
    };

    public static readonly VehicleEntity OneVehicle = UpdateVehicle with
    {
        Id = Guid.Parse("1130b335-07ea-4cbc-b510-0e127325ed1e"),
        OwnerId = UserSeeds.JustVehicleOwnerUser.Id
    };


    public static VehicleEntity GetNoRelationsEntity(VehicleEntity entity)
    {
        return entity with
        {
            Owner = null,
            Rides = new List<RideEntity>()
        };
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleEntity>()
            .HasData(
                Felicia,
                Karosa,
                CascadeDeleteVehicle,
                UpdateVehicle,
                DeleteVehicle,
                OneVehicle
            );
    }
}

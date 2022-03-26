using System;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;
using RideSharing.Common.Enums;

namespace RideSharing.Common.Tests.Seeds;

public static class VehicleSeeds
{
    public static readonly VehicleEntity EmptyVehicle = new(
        Id: default,
        OwnerId: default,
        Type: default,
        Make: default!,
        Model: default!,
        Registered: default,
        Seats: default,
        ImageUrl: default
    );

    public static readonly VehicleEntity Felicia = new(
            Id: Guid.Parse(input: "0a5ef2a1-d541-45ac-82a3-6b63a24d0572"),
            OwnerId: UserSeeds.VehiclesUser.Id,
            Type: VehicleType.Car,
            Make: "Škoda",
            Model: "Felicia",
            Registered: DateTime.Parse("05/16/1996"),
            Seats: 5,
            ImageUrl: "https://auta5p.eu/katalog/skoda/felicia_28.jpg"
        ) {Owner = UserSeeds.VehiclesUser};

    public static readonly VehicleEntity Karosa = new(
            Id: Guid.Parse(input:"14CD044E-F183-47BD-9A4C-68050E9111E7"),
            OwnerId: UserSeeds.VehiclesUser.Id,
            Type: VehicleType.Bus,
            Make: "Karosa",
            Model: "LC 757",
            Registered: DateTime.Parse("03/12/1983"),
            Seats: 20,
            ImageUrl: "https://upload.wikimedia.org/wikipedia/commons/c/c6/Bus_LC757-HD12_Brno.jpg"
        ) {Owner = UserSeeds.VehiclesUser};

    static VehicleSeeds()
    {
        //Felicia.Rides.Add(RideSeeds.PragueBrno);
        //Felicia.Rides.Add(RideSeeds.BrnoBratislava);
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleEntity>().HasData(
            Felicia with {Owner = null},
            Karosa with {Owner = null }
        );
    }
}

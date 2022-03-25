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
        OwnerId: UserSeeds.DriverUser.Id,
        Type: VehicleType.Car,
        Make: "Škoda",
        Model: "Felicia",
        Registered: DateTime.Parse("05/16/1996"),
        Seats: 5,
        ImageUrl: "https://auta5p.eu/katalog/skoda/felicia_28.jpg"
    ){Owner = UserSeeds.DriverUser};



    static VehicleSeeds()
    {
        //Fe.Vehicles.Add(VehicleSeeds.Felicia);
        //DriverUser.Reviews.Add(ReviewSeeds.Perfect);

        //UserSeeds.ReservationUser.Reservations.Add(ReservationSeeds.BrnoTwoSeats);
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleEntity>().HasData(
            Felicia with {Owner = null}
        );
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;
using RideSharing.Common.Tests.Seeds;

namespace RideSharing.Common.Tests.Seeds;

public static class UserSeeds
{
    public static readonly UserEntity EmptyUser = new(
        Id: default,
        Name: default!,
        Surname: default!,
        Phone: default!,
        ImageUrl: default
    );

    public static readonly UserEntity DriverUser = new(
        Id: Guid.Parse(input: "f34cd643-1226-406d-971d-b5e6f745938e"),
        Name: "Driver",
        Surname: "Doe",
        Phone: "737195090",
        ImageUrl: "https://images.pexels.com/photos/771742/pexels-photo-771742.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500"
        );

    public static readonly UserEntity ReservationUser = new(
        Id: Guid.Parse(input: "505b1e64-ed3c-44d1-883e-67de32b3ca59"),
        Name: "Reserve",
        Surname: "Tusk",
        Phone: "585453123",
        ImageUrl: "https://pbs.twimg.com/profile_images/1443129819122782209/sqba2I3D_400x400.jpg"
        );

    static UserSeeds()
    {
        DriverUser.Vehicles.Add(VehicleSeeds.Felicia);
        //DriverUser.Reviews.Add(ReviewSeeds.Perfect);

        //ReservationUser.Reservations.Add(ReservationSeeds.BrnoTwoSeats);
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasData(
            DriverUser with{Vehicles = new List<VehicleEntity>()}, 
            ReservationUser
        );
    }
}

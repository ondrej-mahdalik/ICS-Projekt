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

    public static readonly UserEntity VehiclesUser = new(
        Id: Guid.Parse(input: "f34cd643-1226-406d-971d-b5e6f745938e"),
        Name: "Driver",
        Surname: "Doe",
        Phone: "737195090",
        ImageUrl: "https://images.pexels.com/photos/771742/pexels-photo-771742.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500"
        );

    public static readonly UserEntity CreatedRidesUser = new(
        Id: Guid.Parse(input: "928EF886-B7BA-4082-9773-13B5E9013EBA"),
        Name: "Driver",
        Surname: "Doe",
        Phone: "737195090",
        ImageUrl: "https://images.pexels.com/photos/771742/pexels-photo-771742.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500"
    );

    public static readonly UserEntity ReservationUser1 = new(
        Id: Guid.Parse(input: "EA6574A1-56DE-4436-A5B6-547CF33347AF"),
        Name: "Reserve",
        Surname: "First",
        Phone: "585453123",
        ImageUrl: "https://pbs.twimg.com/profile_images/1443129819122782209/sqba2I3D_400x400.jpg"
        );

    public static readonly UserEntity ReservationUser2 = new(
        Id: Guid.Parse(input: "2F400802-6581-45D3-94CE-8C8EA9DE272D"),
        Name: "Reserve",
        Surname: "Second",
        Phone: "168489635",
        ImageUrl: "https://icons-for-free.com/iconfiles/png/512/person-1324760545186718018.png"
    );

    static UserSeeds()
    {
        VehiclesUser.Vehicles.Add(VehicleSeeds.Felicia);
        VehiclesUser.Vehicles.Add(VehicleSeeds.Karosa);

        CreatedRidesUser.CreatedRides.Add(RideSeeds.PragueBrno);
        CreatedRidesUser.CreatedRides.Add(RideSeeds.BrnoBratislava);

        ReservationUser1.Reservations.Add(ReservationSeeds.User1BrnoBratislava);
        ReservationUser1.Reservations.Add(ReservationSeeds.User1PragueBrno);

        ReservationUser2.Reservations.Add(ReservationSeeds.User2PragueBrno);
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasData(
            VehiclesUser with {Vehicles = new List<VehicleEntity>()},
            CreatedRidesUser with {CreatedRides = new List<RideEntity>() },
            ReservationUser1 with {Reservations = new List<ReservationEntity>()}, 
            ReservationUser2 with {Reservations = new List<ReservationEntity>() }

        );
    }
}

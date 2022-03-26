using System;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.Common.Tests.Seeds;

public static class RideSeeds
{
    public static readonly RideEntity EmptyRideEntity = new(
        Id: default,
        FromName: default!,
        FromLatitude: default,
        FromLongitude: default,
        ToName: default!,
        ToLatitude: default,
        ToLongitude: default,
        Departure: default,
        Arrival: default,
        DriverId: default,
        VehicleId: default,
        Note: default
    );

    public static readonly RideEntity PragueBrno = new(
        Id: Guid.Parse("42b612c1-b668-4168-9b73-71acfb64f094"),
        FromName: "Prague",
        FromLatitude: 50.07698467371664,
        FromLongitude: 14.432483187893586,
        ToName: "Brno",
        ToLatitude: 49.22611604448722,
        ToLongitude: 16.582455843955017,
        Departure: DateTime.Parse("02/22/2022 17:30"),
        Arrival: DateTime.Parse("02/22/2022 19:40"),
        DriverId: UserSeeds.CreatedRidesUser.Id,
        VehicleId: VehicleSeeds.Felicia.Id,
        Note: "No eating in the car!"
    )
    {
        Driver = UserSeeds.CreatedRidesUser,
    };


    public static readonly RideEntity BrnoBratislava = new(
        Id: Guid.Parse(input: "88E52AEF-B1B5-4E41-A916-CC810E1FD305"),
        FromName: "Brno",
        FromLatitude: 50.07698467371664,
        FromLongitude: 14.432483187893586,
        ToName: "Bratislava",
        ToLatitude: 49.22611604448722,
        ToLongitude: 16.582455843955017,
        Departure: DateTime.Parse("06/15/2022 15:00"),
        Arrival: DateTime.Parse("06/15/2022 18:00"),
        DriverId: UserSeeds.CreatedRidesUser.Id,
        VehicleId: VehicleSeeds.Felicia.Id,
        Note: "Eating in the car is allowed!"
    ){
        Driver = UserSeeds.CreatedRidesUser,
    };

    static RideSeeds()
    {
        //PragueBrno.Reservations.Add(ReservationSeeds.User1PragueBrno);
        //PragueBrno.Reservations.Add(ReservationSeeds.User2PragueBrno);

        //BrnoBratislava.Reservations.Add(ReservationSeeds.User1BrnoBratislava);
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RideEntity>().HasData(
            PragueBrno with {Driver = null, Vehicle = null, Reservations = new List<ReservationEntity>()},
            BrnoBratislava with {Driver = null, Vehicle = null, Reservations = new List<ReservationEntity>()}

        );
    }

}

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
        Departure: DateTime.Parse("02/22/2022 15:00"),
        Arrival: DateTime.Parse("02/22/2022 18:00"),
        DriverId: UserSeeds.DriverUser.Id,
        VehicleId: VehicleSeeds.Felicia.Id,
        Note: "No eating in the car !"
    );

    static RideSeeds()
    {
       // PragueBrno.Reservations.Add(ReservationSeeds.BrnoTwoSeats);
       // PragueBrno.Reviews.Add(ReviewSeeds.Perfect);
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RideEntity>().HasData(
            PragueBrno
        );
    }
}

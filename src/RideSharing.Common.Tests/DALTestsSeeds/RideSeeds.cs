using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.Common.Tests.DALTestsSeeds;

public static class RideSeeds
{
    public static readonly RideEntity EmptyRideEntity = new(
        Id: default,
        SharedSeats: default,
        FromName: default!,
        FromLatitude: default,
        FromLongitude: default,
        Distance:default,
        ToName: default!,
        ToLatitude: default,
        ToLongitude: default,
        Departure: default,
        Arrival: default,
        VehicleId: default,
        Note: default
    );

    public static readonly RideEntity PragueBrno = new(
        Id: Guid.Parse("42b612c1-b668-4168-9b73-71acfb64f094"),
        SharedSeats: 4,
        FromName: "Prague",
        FromLatitude: 50.07698467371664,
        FromLongitude: 14.432483187893586,
        Distance: 207,
        ToName: "Brno",
        ToLatitude: 49.22611604448722,
        ToLongitude: 16.582455843955017,
        Departure: DateTime.Parse("02/22/2022 17:30", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("02/22/2022 19:40", CultureInfo.InvariantCulture),
        VehicleId: VehicleSeeds.Felicia.Id,
        Note: "No eating in the car!"
    );

    public static readonly RideEntity BrnoBratislava = new(
        Id: Guid.Parse(input: "88E52AEF-B1B5-4E41-A916-CC810E1FD305"),
        SharedSeats: 2,
        FromName: "Brno",
        FromLatitude: 50.07698467371664,
        FromLongitude: 14.432483187893586,
        Distance: 130,
        ToName: "Bratislava",
        ToLatitude: 49.22611604448722,
        ToLongitude: 16.582455843955017,
        Departure: DateTime.Parse("06/15/2022 15:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("06/15/2022 18:00", CultureInfo.InvariantCulture),
        VehicleId: VehicleSeeds.Felicia.Id,
        Note: "Eating in the car is allowed!"
    );

    public static readonly RideEntity CascadeDeleteRide = GetNoRelationsEntity(PragueBrno) with
    {
        Id = Guid.Parse("1f459e21-ce03-4bfa-9794-25d09b231ba8"),
        VehicleId = VehicleSeeds.CascadeDeleteVehicle.Id
    };

    public static readonly RideEntity UpdateRide = GetNoRelationsEntity(CascadeDeleteRide) with
    {
        Id = Guid.Parse("265726e1-7408-4178-84e4-3e1f2fb4d0d7")
    };

    public static readonly RideEntity DeleteRide = GetNoRelationsEntity(CascadeDeleteRide) with
    {
        Id = Guid.Parse("bdd94aaf-7dac-4d9c-badd-6a1eeb24e380")
    };
    public static RideEntity GetNoRelationsEntity(RideEntity entity)
    {
        return entity with
        {
            Vehicle = null,
            Reservations = new List<ReservationEntity>(),
            Reviews = new List<ReviewEntity>()
        };
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RideEntity>().HasData(
            PragueBrno,
            BrnoBratislava,
            CascadeDeleteRide,
            UpdateRide,
            DeleteRide
        );
    }

}

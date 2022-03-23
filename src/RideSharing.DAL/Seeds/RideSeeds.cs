using System;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.DAL.Seeds;

public static class RideSeeds
{
    public static readonly RideEntity PrahaBrno = new(
        Id: Guid.Parse("42b612c1-b668-4168-9b73-71acfb64f094"),
        FromName: "Praha",
        FromLatitude: 50.07698467371664,
        FromLongitude: 14.432483187893586,
        ToName: "Brno",
        ToLatitude: 49.22611604448722,
        ToLongitude: 16.582455843955017,
        Distance: 350,
        Departure: DateTime.Parse("22.2.2022 15:00"),
        Arrival: DateTime.Parse("22.2.2022 18:00"),
        DriverId: Guid.Parse(input: "f34cd643-1226-406d-971d-b5e6f745938e"),
        VehicleId: Guid.Parse(input: "0a5ef2a1-d541-45ac-82a3-6b63a24d0572"),
        Note: "No eating in the car !"
    );

    static RideSeeds()
    {
        //PrahaBrno.Reservations.Add(ReservationSeeds.BrnoTwoSeats);
        //PrahaBrno.Reviews.Add(ReviewSeeds.Perfect);
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RideEntity>().HasData(
            PrahaBrno
        );
    }
}

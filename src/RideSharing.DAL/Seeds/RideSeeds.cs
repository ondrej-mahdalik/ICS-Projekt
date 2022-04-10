using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.DAL.Seeds;

public static class RideSeeds
{
    public static readonly RideEntity PrahaBrno = new(
        Guid.Parse("42b612c1-b668-4168-9b73-71acfb64f094"),
        FromName: "Praha",
        FromLatitude: 50.07698467371664,
        FromLongitude: 14.432483187893586,
        ToName: "Brno",
        ToLatitude: 49.22611604448722,
        ToLongitude: 16.582455843955017,
        Distance: 350,
        SharedSeats: 4,
        Departure: DateTime.Parse("02/22/2022 15:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("02/22/2022 18:00", CultureInfo.InvariantCulture),
        VehicleId: Guid.Parse("0a5ef2a1-d541-45ac-82a3-6b63a24d0572"),
        Note: "No eating in the car !"
    );

    static RideSeeds()
    {
        //PrahaBrno.Reservations.Add(ReservationSeeds.BrnoTwoSeats);
        //PrahaBrno.ReceivedReviews.Add(ReviewSeeds.Perfect);
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RideEntity>().HasData(
            PrahaBrno
        );
    }
}

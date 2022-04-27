using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.DAL.Seeds;

public static class RideSeeds
{
    public static readonly RideEntity PrahaBrno = new(
        Guid.Parse("42b612c1-b668-4168-9b73-71acfb64f094"),
        FromName: "Prague",
        FromLatitude: 50.07698467371664,
        FromLongitude: 14.432483187893586,
        ToName: "Brno",
        ToLatitude: 49.22611604448722,
        ToLongitude: 16.582455843955017,
        Distance: 350,
        SharedSeats: 4,
        Departure: DateTime.Parse("09/22/2022 15:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("09/22/2022 18:00", CultureInfo.InvariantCulture),
        VehicleId: VehicleSeeds.Felicia.Id,
        Note: "No eating in the car !"
    );


    public static readonly RideEntity BrnoOstrava = new(
        Guid.Parse("a29fd7d3-bba1-496c-bcbb-5df70a575a9c"),
        FromName: "Prague",
        FromLatitude: 50.07698467371664,
        FromLongitude: 14.432483187893586,
        ToName: "Ostrava",
        ToLatitude: 49.22611604448722,
        ToLongitude: 16.582455843955017,
        Distance: 350,
        SharedSeats: 4,
        Departure: DateTime.Parse("01/22/2022 15:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("01/22/2022 18:00", CultureInfo.InvariantCulture),
        VehicleId: VehicleSeeds.Felicia.Id,
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
            PrahaBrno,
            BrnoOstrava
        );
    }
}

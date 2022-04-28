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

    public static readonly RideEntity OlomoucOstrava = new(
        Guid.Parse("42a232a6-b668-4567-9b73-71gffb64a075"),
        FromName: "Olomouc",
        FromLatitude: 49.5891231,
        FromLongitude: 17.2538600,
        ToName: "Ostrava",
        ToLatitude: 49.8050306,
        ToLongitude: 18.2392447,
        Distance: 96,
        SharedSeats: 15,
        Departure: DateTime.Parse("01/01/2022 12:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("01/01/2022 13:00", CultureInfo.InvariantCulture),
        VehicleId: Seeds.VehicleSeeds.Karosa.Id,
        Note: null
    );

    public static readonly RideEntity OlomoucBrno = new(
        Guid.Parse("42a232a6-a668-abcd-1234-71gffb64a054"),
        FromName: "Olomouc",
        FromLatitude: 49.5891231,
        FromLongitude: 17.2538600,
        ToName: "Brno",
        ToLatitude: 49.22611604448722,
        ToLongitude: 16.582455843955017,
        Distance: 77,
        SharedSeats: 1,
        Departure: DateTime.Parse("05/21/2022 18:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("05/21/2022 18:50", CultureInfo.InvariantCulture),
        VehicleId: Seeds.VehicleSeeds.Felicia.Id,
        Note: "There will be a dog in the car."
    );

    static RideSeeds()
    {
        //PrahaBrno.Reservations.Add(ReservationSeeds.BrnoTwoSeats);
        //PrahaBrno.ReceivedReviews.Add(ReviewSeeds.Perfect);
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RideEntity>().HasData(
            PrahaBrno, BrnoOstrava, OlomoucOstrava, OlomoucBrno
        );
    }
}

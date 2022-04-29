using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.DAL.Seeds;

public static class RideSeeds
{
    public static readonly RideEntity PrahaBrno = new(
        Guid.Parse("42b612c1-b668-4168-9b73-71acfb64f094"),
        FromName: "Prague",
        ToName: "Brno",
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
        ToName: "Ostrava",
        Distance: 350,
        SharedSeats: 4,
        Departure: DateTime.Parse("01/22/2022 15:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("01/22/2022 18:00", CultureInfo.InvariantCulture),
        VehicleId: VehicleSeeds.Felicia.Id,
        Note: "No eating in the car !"
    );

    public static readonly RideEntity OlomoucOstrava = new(
        Guid.Parse("42a232a6-b668-4567-9b73-71cffb64a075"),
        FromName: "Olomouc",
        ToName: "Ostrava",
        Distance: 96,
        SharedSeats: 15,
        Departure: DateTime.Parse("01/01/2022 12:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("01/01/2022 13:00", CultureInfo.InvariantCulture),
        VehicleId: Seeds.VehicleSeeds.Karosa.Id,
        Note: null
    );

    public static readonly RideEntity OlomoucBrno = new(
        Guid.Parse("42a232a6-a668-abcd-1234-71affb64a054"),
        FromName: "Olomouc",
        ToName: "Brno",
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

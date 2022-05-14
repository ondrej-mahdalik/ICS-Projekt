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
        FromName: "Brno",
        ToName: "Ostrava",
        Distance: 350,
        SharedSeats: 4,
        Departure: DateTime.Parse("01/22/2022 15:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("01/22/2022 18:00", CultureInfo.InvariantCulture),
        VehicleId: VehicleSeeds.Felicia.Id,
        Note: "No eating in the car !"
    );

    public static readonly RideEntity BrnoOstrava2 = new(
        Guid.Parse("c025a59b-e7a6-45a8-8c73-81eea2780615"),
        FromName: "Brno",
        ToName: "Ostrava",
        Distance: 169,
        SharedSeats: 4,
        Departure: DateTime.Parse("11/22/2022 16:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("11/22/2022 18:00", CultureInfo.InvariantCulture),
        VehicleId: VehicleSeeds.Felicia.Id,
        Note: "No eating in the car !"
    );

    public static readonly RideEntity BrnoPrague = new(
        Guid.Parse("49ba15a1-0ce2-43db-bf39-c4bcb9202f60"),
        FromName: "Brno",
        ToName: "Prague",
        Distance: 234,
        SharedSeats: 4,
        Departure: DateTime.Parse("10/22/2022 13:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("10/22/2022 15:40", CultureInfo.InvariantCulture),
        VehicleId: VehicleSeeds.Felicia.Id,
        Note: "No eating in the car !"
    );

    public static readonly RideEntity BrnoOlomouc = new(
        Guid.Parse("ddd2cb70-6d85-4e77-8eeb-2e6cccca6b62"),
        FromName: "Brno",
        ToName: "Olomouc",
        Distance: 79,
        SharedSeats: 4,
        Departure: DateTime.Parse("9/15/2022 10:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("9/15/2022 10:57", CultureInfo.InvariantCulture),
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

    public static readonly RideEntity TynecOlomouc = new(
        Guid.Parse("31fa8947-bc4e-4316-aa5b-d0bbf879b5ee"),
        FromName: "Velký Týnec",
        ToName: "Olomouc",
        Distance: 15,
        SharedSeats: 1,
        Departure: DateTime.Parse("06/04/2020 15:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("06/04/2020 18:00", CultureInfo.InvariantCulture),
        VehicleId: VehicleSeeds.Bicycle.Id,
        Note: "Take your own bicycle helmet."
    );

    public static readonly RideEntity TynecPraha = new(
        Guid.Parse("28cd9961-b5bb-4064-88fa-2bc907f69b06"),
        FromName: "Velký Týnec",
        ToName: "Praha",
        Distance: 288,
        SharedSeats: 7,
        Departure: DateTime.Parse("2021-02-05 12:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("2021-02-15 14:00", CultureInfo.InvariantCulture),
        VehicleId: VehicleSeeds.Van.Id,
        Note: "We may need to do some repairs on the way. Bring a screwdriver and a hammer."
    );

    public static readonly RideEntity ElonAsOstrava = new(
        Guid.Parse("d4c33221-2196-40d8-b45c-49ec23a627fd"),
        FromName: "Aš",
        ToName: "Ostrava",
        Distance: 564,
        SharedSeats: 19,
        Departure: DateTime.Parse("2023-02-05 12:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("2023-02-15 18:00", CultureInfo.InvariantCulture),
        VehicleId: VehicleSeeds.Karosa.Id,
        Note: "Toilet is broken."
    );

    static RideSeeds()
    {
        //PrahaBrno.Reservations.Add(ReservationSeeds.BrnoTwoSeats);
        //PrahaBrno.ReceivedReviews.Add(ReviewSeeds.Perfect);
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RideEntity>().HasData(
            PrahaBrno, BrnoOstrava, OlomoucOstrava, OlomoucBrno, TynecOlomouc, TynecPraha, BrnoOlomouc, BrnoPrague, BrnoOstrava2, ElonAsOstrava
        );
    }
}

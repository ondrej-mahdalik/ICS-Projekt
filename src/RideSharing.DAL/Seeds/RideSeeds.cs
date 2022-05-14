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
        VehicleId: VehicleSeeds.AudiA8.Id,
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

    public static readonly RideEntity NovakPragueOstrava = new(
        Id: Guid.Parse("1768425c-17d0-4b6f-8f54-fb242949fa1a"),
        FromName: "Prague",
        ToName: "Ostrava",
        Distance: 371,
        SharedSeats: 5,
        Departure: DateTime.Parse("2023-05-05 11:11", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("2023-05-05 14:44", CultureInfo.InvariantCulture),
        VehicleId: VehicleSeeds.Van.Id,
        Note: null
        );
    
    public static readonly RideEntity DoeJihlavaOstrava = new(
        Id: Guid.Parse("bb420321-1f68-4ff0-9204-95880c084dff"),
        FromName: "Jihlava",
        ToName: "Ostrava",
        Distance: 253,
        SharedSeats: 1,
        Departure: DateTime.Parse("2021-04-01 12:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("2021-04-01 14:22", CultureInfo.InvariantCulture),
        VehicleId: VehicleSeeds.YBR125.Id,
        Note: "Bring a helmet!"
    );

    // 3 past rides for all users that have a vehicle
    // These have a reservation and a review, so all user that can share a ride have a rating
    public static readonly RideEntity TuskJihlavaMohelnice = new(
        Id: Guid.Parse("fe78f17d-e9e2-448f-ab15-68b2b14e06bc"),
        FromName: "Jihlava",
        ToName: "Mohelnice",
        Distance: 198,
        SharedSeats: 5,
        Departure: DateTime.Parse("2020-01-01 08:00",CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("2020-01-01 09:55", CultureInfo.InvariantCulture),
        VehicleId: VehicleSeeds.Karosa.Id,
        Note: null
    );

    public static readonly RideEntity NovakJihlavaBrno = new(
        Id: Guid.Parse("58698380-09c7-4e88-9839-39ed1428b2f6"),
        FromName: "Jihlava",
        ToName: "Brno",
        Distance: 89,
        SharedSeats: 2,
        Departure: DateTime.Parse("2021-02-02 14:02",CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("2021-02-02 15:02",CultureInfo.InvariantCulture),
        VehicleId: VehicleSeeds.AudiA8.Id,
        Note: null
    );

    public static readonly RideEntity DoeJihlavaOlomouc = new(
        Id: Guid.Parse("d8108381-5dcd-4237-b621-41a21fc56734"),
        FromName: "Jihlava",
        ToName: "Olomouc",
        Distance: 167,
        SharedSeats: 1,
        Departure: DateTime.Parse("2020-04-08 09:00", CultureInfo.InvariantCulture),
        Arrival: DateTime.Parse("2020-04-08 11:00", CultureInfo.InvariantCulture),
        VehicleId: VehicleSeeds.Felicia.Id,
        Note: null
    );

    static RideSeeds()
    {
        //PrahaBrno.Reservations.Add(ReservationSeeds.BrnoTwoSeats);
        //PrahaBrno.ReceivedReviews.Add(ReviewSeeds.Perfect);
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RideEntity>().HasData(
            PrahaBrno, BrnoOstrava, OlomoucOstrava, OlomoucBrno, TynecOlomouc,
            TynecPraha, BrnoOlomouc, BrnoPrague, BrnoOstrava2, ElonAsOstrava,
            NovakPragueOstrava, DoeJihlavaOstrava, TuskJihlavaMohelnice, NovakJihlavaBrno,
            DoeJihlavaOlomouc
        );
    }
}

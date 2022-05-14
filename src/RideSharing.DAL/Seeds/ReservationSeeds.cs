using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;
using RideSharing.DAL.Seeds;

namespace RideSharing.DAL.Seeds;

public static class ReservationSeeds
{
    public static readonly ReservationEntity PrahaBrnoJanNovotnny = new(
        Guid.Parse("7c6cdd3e-481d-427a-a971-ae306aba8c95"),
        Seeds.UserSeeds.JanNovotny.Id,
        RideSeeds.PrahaBrno.Id,
        2,
        DateTime.Parse("03/02/2022 00:00", CultureInfo.InvariantCulture)
    );

    public static readonly ReservationEntity OlomoucOstravaJohnDoe = new(
        Guid.Parse("a9b1e70b-1cde-471b-990c-1230aa649d0a"),
        Seeds.UserSeeds.JohnDoe.Id,
        RideSeeds.OlomoucOstrava.Id,
        1,
        DateTime.Parse("12/03/2021 12:30", CultureInfo.InvariantCulture)
    );

    public static readonly ReservationEntity OlomoucBrnoJanNovotny = new(
        Guid.Parse("35773f2e-3dfa-415d-8440-e637a09ac7d1"),
        Seeds.UserSeeds.JanNovotny.Id,
        RideSeeds.OlomoucBrno.Id,
        1,
        DateTime.Parse("05/02/2022 13:36", CultureInfo.InvariantCulture)
    );

    public static readonly ReservationEntity OlomoucOstravaJanNovotny = new(
        Guid.Parse("bcb31a99-dc00-4243-a245-c79d62e1a157"),
        Seeds.UserSeeds.JanNovotny.Id,
        RideSeeds.OlomoucOstrava.Id,
        3,
        DateTime.Parse("12/30/2021 13:36", CultureInfo.InvariantCulture)
    );

    public static readonly ReservationEntity PrahaBrnoPavelNovak = new(
        Guid.Parse("0edec00c-3973-457c-a0a3-31de8dd15219"),
        Seeds.UserSeeds.PavelNovak.Id,
        RideSeeds.PrahaBrno.Id,
        2,
        DateTime.Parse("01/01/2022 12:55", CultureInfo.InvariantCulture)
    );

    public static readonly ReservationEntity OlomoucOstravaPavelNovak = new(
        Guid.Parse("24834e31-2a64-4d82-aaf7-cda64340916e"),
        UserSeeds.PavelNovak.Id,
        RideSeeds.OlomoucOstrava.Id,
        1,
        DateTime.Parse("12/25/2021 12:00", CultureInfo.InvariantCulture)
    );

    public static readonly ReservationEntity TynecOlomoucElonTusk = new(
        Guid.Parse("573c268e-296e-43fa-bb5b-83da3b250416"),
        UserSeeds.ElonTusk.Id,
        RideSeeds.TynecOlomouc.Id,
        1,
        DateTime.Parse("06/03/2020 15:00", CultureInfo.InvariantCulture)
    );

    public static readonly ReservationEntity TynecPrahaElonTusk = new(
        Id: Guid.Parse("70a1898b-6d84-458e-aa02-ccfa09856fb6"),
        UserSeeds.ElonTusk.Id,
        RideSeeds.TynecPraha.Id,
        2,
        DateTime.Parse("2021-02-02 12:00", CultureInfo.InvariantCulture)
    );

    public static readonly ReservationEntity TynecPrahaJohnDoe = new(
        Id: Guid.Parse("d3c0e43e-e695-484f-b15e-9d9ccc86b85a"),
        UserSeeds.JohnDoe.Id,
        RideSeeds.TynecPraha.Id,
        1,
        DateTime.Parse("2021-02-01 18:25", CultureInfo.InvariantCulture)
    );

    public static readonly ReservationEntity TynecPrahaJanNovotny = new(
        Id: Guid.Parse("1fc8fb6a-22f7-41c0-b355-3f343a9db1c8"),
        UserSeeds.JanNovotny.Id,
        RideSeeds.TynecPraha.Id,
        1,
        DateTime.Parse("2021-02-02 11:00", CultureInfo.InvariantCulture)
    );

    public static readonly ReservationEntity AsOstravaJonDoe = new(
        Id: Guid.Parse("203ffbdc-98a1-47cb-8124-693e74a39dfc"),
        UserSeeds.JohnDoe.Id,
        RideSeeds.ElonAsOstrava.Id,
        6,
        DateTime.Parse("2022-02-01 18:25", CultureInfo.InvariantCulture)
    );



    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReservationEntity>().HasData(
            PrahaBrnoJanNovotnny, OlomoucOstravaJohnDoe, OlomoucBrnoJanNovotny, OlomoucOstravaJanNovotny, PrahaBrnoPavelNovak, OlomoucOstravaPavelNovak, TynecOlomoucElonTusk, TynecPrahaElonTusk, TynecPrahaJohnDoe, TynecPrahaJanNovotny, AsOstravaJonDoe
        );
    }
}

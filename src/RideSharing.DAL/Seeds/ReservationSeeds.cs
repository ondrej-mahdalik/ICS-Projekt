using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;
using RideSharing.DAL.Seeds;

namespace RideSharing.DAL.Seeds;

public static class ReservationSeeds
{
    public static readonly ReservationEntity BrnoTwoSeats = new(
        Guid.Parse("7c6cdd3e-481d-427a-a971-ae306aba8c95"),
        Seeds.UserSeeds.JohnDoe.Id,
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

    public static readonly ReservationEntity OlomoucBrnoJohnDoe = new(
        Guid.Parse("35773f2e-3dfa-415d-8440-e637a09ac7d1"),
        Seeds.UserSeeds.JohnDoe.Id,
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


    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReservationEntity>().HasData(
            BrnoTwoSeats, OlomoucOstravaJohnDoe, OlomoucBrnoJohnDoe, OlomoucOstravaJanNovotny, PrahaBrnoPavelNovak, OlomoucOstravaPavelNovak
        );
    }
}

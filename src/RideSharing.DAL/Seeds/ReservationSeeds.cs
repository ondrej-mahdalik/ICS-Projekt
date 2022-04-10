using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.DAL.Seeds;

public static class ReservationSeeds
{
    public static readonly ReservationEntity BrnoTwoSeats = new(
        Guid.Parse("82c202d6-260b-4e41-91b0-bde77d75a00a"),
        Guid.Parse("505b1e64-ed3c-44d1-883e-67de32b3ca59"),
        RideSeeds.PrahaBrno.Id,
        2,
        DateTime.Parse("03/02/2022 00:00", CultureInfo.InvariantCulture)
    );

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReservationEntity>().HasData(
            BrnoTwoSeats
        );
    }
}

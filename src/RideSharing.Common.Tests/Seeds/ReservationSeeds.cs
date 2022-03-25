using System;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.Common.Tests.Seeds;

public static class ReservationSeeds
{
    public static readonly ReservationEntity BrnoTwoSeats = new(
        Id: Guid.Parse("82c202d6-260b-4e41-91b0-bde77d75a00a"),
        UserId: UserSeeds.ReservationUser.Id,
        RideId: RideSeeds.PragueBrno.Id,
        Seats: 2,
        Timestamp: DateTime.Parse("02/20/2022")
    );

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReservationEntity>().HasData(
            BrnoTwoSeats
        );
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.Common.Tests.Seeds;

public static class ReservationSeeds
{
    public static readonly ReservationEntity BrnoTwoSeats = new(
        Id: Guid.Parse("82c202d6-260b-4e41-91b0-bde77d75a00a"),
        ReservingUserId: Guid.Parse(input: "505b1e64-ed3c-44d1-883e-67de32b3ca59"),
        RideId: Guid.Parse("42b612c1-b668-4168-9b73-71acfb64f094"),
        Seats: 2,
        Timestamp: DateTime.Parse("20.02.2022")
    );

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReservationEntity>().HasData(
            BrnoTwoSeats
        );
    }
}

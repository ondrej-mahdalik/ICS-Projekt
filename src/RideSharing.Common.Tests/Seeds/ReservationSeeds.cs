using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.Common.Tests.Seeds;

public static class ReservationSeeds
{
    public static readonly ReservationEntity BrnoTwoSeats = new(
        Id: Guid.Parse("92b1c6a4-b399-4c28-8bdc-299ff448ada3"),
        ReservingUserId: UserSeeds.ElonTusk.Id,
        RideId: RideSeeds.PrahaBrno.Id,
        Seats: 2,
        Timestamp: DateTime.Parse("02/22/2022 14:00", CultureInfo.InvariantCulture)
    );

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReservationEntity>().HasData(
            BrnoTwoSeats
        );
    }
}

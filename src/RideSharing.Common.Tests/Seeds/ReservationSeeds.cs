using System;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.Common.Tests.Seeds;

public static class ReservationSeeds
{
    public static readonly ReservationEntity User1PragueBrno = new(
        Id: Guid.Parse(input: "82c202d6-260b-4e41-91b0-bde77d75a00a"),
        UserId: UserSeeds.ReservationUser1.Id,
        RideId: RideSeeds.PragueBrno.Id,
        Seats: 2,
        Timestamp: DateTime.Parse("02/20/2022 12:20")
    )
    {
        Ride = RideSeeds.PragueBrno,
        User = UserSeeds.ReservationUser1
    };

    public static readonly ReservationEntity User2PragueBrno = new(
        Id: Guid.Parse(input: "42D7E8ED-CB6D-4B3B-8B5F-7FA002FE7D82"),
        UserId: UserSeeds.ReservationUser2.Id,
        RideId: RideSeeds.PragueBrno.Id,
        Seats: 1,
        Timestamp: DateTime.Parse("02/20/2022 13:40")
    )
    {
        Ride = RideSeeds.PragueBrno,
        User = UserSeeds.ReservationUser2
    };

    public static readonly ReservationEntity User1BrnoBratislava = new(
        Id: Guid.Parse(input: "61233F0D-A0D5-4287-AE7C-6BC8A9AF8C5B"),
        UserId: UserSeeds.ReservationUser1.Id,
        RideId: RideSeeds.BrnoBratislava.Id,
        Seats: 4,
        Timestamp: DateTime.Parse("04/28/2022 11:10")
    )
    {
        Ride = RideSeeds.BrnoBratislava,
        User = UserSeeds.ReservationUser1
    };



    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReservationEntity>().HasData(
            User1PragueBrno with {Ride = null, User = null},
            User2PragueBrno with { Ride = null, User = null},
            User1BrnoBratislava with { Ride = null, User = null }
        );
    }
}

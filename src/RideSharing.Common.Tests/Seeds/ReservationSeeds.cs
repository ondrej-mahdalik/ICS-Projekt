using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.Common.Tests.Seeds;

public static class ReservationSeeds
{
    public static readonly ReservationEntity EmptyReservation = new(default,
        default,
        default,
        default,
        default);

    public static readonly ReservationEntity User1PragueBrno = new(
        Guid.Parse("82c202d6-260b-4e41-91b0-bde77d75a00a"),
        UserSeeds.ReservationUser1.Id,
        RideSeeds.PragueBrno.Id,
        2,
        DateTime.Parse("02/20/2022 12:20", CultureInfo.InvariantCulture));

    public static readonly ReservationEntity User2PragueBrno = new(
        Guid.Parse("42D7E8ED-CB6D-4B3B-8B5F-7FA002FE7D82"),
        UserSeeds.ReservationUser2.Id,
        RideSeeds.PragueBrno.Id,
        1,
        DateTime.Parse("02/20/2022 13:40", CultureInfo.InvariantCulture));

    public static readonly ReservationEntity User1BrnoBratislava = new(
        Guid.Parse("61233F0D-A0D5-4287-AE7C-6BC8A9AF8C5B"),
        UserSeeds.ReservationUser1.Id,
        RideSeeds.BrnoBratislava.Id,
        4,
        DateTime.Parse("04/28/2022 11:10", CultureInfo.InvariantCulture));

    public static readonly ReservationEntity DriverBrnoBratislava = new(
        Guid.Parse("E0C898B4-AF2B-4B82-9AF1-0013BDB47C7B"),
        UserSeeds.DriverUser.Id,
        RideSeeds.BrnoBratislava.Id,
        4,
        DateTime.Parse("04/28/2022 11:10", CultureInfo.InvariantCulture));

    public static readonly ReservationEntity CascadeDeleteReservation = GetNoRelationsEntity(User1BrnoBratislava) with
    {
        Id = Guid.Parse("6638d07e-5bf5-493c-94e3-dfa9956faf45"),
        RideId = RideSeeds.BrnoBratislava.Id,
        ReservingUserId = UserSeeds.CascadeDeleteUser.Id
    };

    public static readonly ReservationEntity UpdateReservation = GetNoRelationsEntity(CascadeDeleteReservation) with
    {
        Id = Guid.Parse("894bbc2f-88f3-4bd2-bcc1-1d2bf36ddcc5")
    };

    public static readonly ReservationEntity DeleteReservation = GetNoRelationsEntity(CascadeDeleteReservation) with
    {
        Id = Guid.Parse("dc817be4-b2ec-46bf-800b-4cf30d829fc9")
    };

    public static readonly ReservationEntity JustOneReservation = GetNoRelationsEntity(CascadeDeleteReservation) with
    {
        Id = Guid.Parse("6fee7f0b-295a-4273-8d56-43d132da2db6"),
        ReservingUserId = UserSeeds.JustReservationOwnerUser.Id
    };

    public static readonly ReservationEntity JustOneRideReservation =
        GetNoRelationsEntity(CascadeDeleteReservation) with
        {
            Id = Guid.Parse("4a69ed97-744e-484b-9be0-d251b3d8d47e"),
            ReservingUserId = UserSeeds.CascadeDeleteUser.Id,
            RideId = RideSeeds.JustReservationRide.Id
        };

    public static ReservationEntity GetNoRelationsEntity(ReservationEntity entity)
    {
        return entity with { ReservingUser = null, Ride = null };
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReservationEntity>()
            .HasData(User1PragueBrno,
                User2PragueBrno,
                User1BrnoBratislava,
                DriverBrnoBratislava,
                CascadeDeleteReservation,
                UpdateReservation,
                DeleteReservation,
                JustOneReservation,
                JustOneRideReservation);
    }
}

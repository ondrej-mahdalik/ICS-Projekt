using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.Common.Tests.Seeds;

public static class UserSeeds
{
    public static readonly UserEntity EmptyUser = new(
        default,
        default!,
        default!,
        default!,
        default
    );

    public static readonly UserEntity DriverUser = new(
        Guid.Parse("f34cd643-1226-406d-971d-b5e6f745938e"),
        "Driver",
        "Doe",
        "737195090",
        "https://images.pexels.com/photos/771742/pexels-photo-771742.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500"
    );

    public static readonly UserEntity ReservationUser1 = new(
        Guid.Parse("EA6574A1-56DE-4436-A5B6-547CF33347AF"),
        "Reserve",
        "First",
        "585453123",
        "https://pbs.twimg.com/profile_images/1443129819122782209/sqba2I3D_400x400.jpg"
    );

    public static readonly UserEntity ReservationUser2 = new(
        Guid.Parse("2F400802-6581-45D3-94CE-8C8EA9DE272D"),
        "Reserve",
        "Second",
        "168489635",
        "https://icons-for-free.com/iconfiles/png/512/person-1324760545186718018.png"
    );

    public static readonly UserEntity UserUpdate = GetNoRelationsEntity(DriverUser) with
    {
        Id = Guid.Parse("BD566AB0-FE85-4E3A-B773-9AC69BCFA3EF")
    };

    public static readonly UserEntity UserDelete = GetNoRelationsEntity(DriverUser) with
    {
        Id = Guid.Parse("1A3431A4-C848-4C73-91FD-4D6835A19D23")
    };

    public static readonly UserEntity CascadeDeleteUser = GetNoRelationsEntity(DriverUser) with
    {
        Id = Guid.Parse("f8d62aa5-d373-4ece-8b9f-989e3f2acaf2")
    };

    public static readonly UserEntity VehicleTestsUser = GetNoRelationsEntity(DriverUser) with
    {
        Id = Guid.Parse("d71772c9-7d37-4c0d-88aa-f6fcb646bb1d")
    };

    public static readonly UserEntity JustVehicleOwnerUser = GetNoRelationsEntity(DriverUser) with
    {
        Id = Guid.Parse("df6309ab-2e10-414d-bbe1-1d795ee866eb")
    };

    public static readonly UserEntity JustReservationOwnerUser = GetNoRelationsEntity(DriverUser) with
    {
        Id = Guid.Parse("e1dbc07b-6d51-4304-883e-bc04e13f6ff9")
    };

    public static readonly UserEntity JustObtainedReviewUser = GetNoRelationsEntity(DriverUser) with
    {
        Id = Guid.Parse("d103779f-0bb7-4c33-8251-c5d42efdcc4b")
    };

    public static readonly UserEntity JustSubmittedReviewUser = GetNoRelationsEntity(DriverUser) with
    {
        Id = Guid.Parse("d18123fe-a865-4f73-9af2-00bdcbee98a6")
    };


    public static UserEntity GetNoRelationsEntity(UserEntity entity)
    {
        return entity with
        {
            SubmittedReviews = new List<ReviewEntity>(),
            Vehicles = new List<VehicleEntity>(),
            Reservations = new List<ReservationEntity>()
        };
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .HasData(
                DriverUser,
                ReservationUser1,
                ReservationUser2,
                UserUpdate,
                UserDelete,
                CascadeDeleteUser,
                VehicleTestsUser,
                JustVehicleOwnerUser,
                JustReservationOwnerUser,
                JustObtainedReviewUser,
                JustSubmittedReviewUser
            );
    }
}

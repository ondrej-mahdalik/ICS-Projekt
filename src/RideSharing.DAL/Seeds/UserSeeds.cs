using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.DAL.Seeds;

public static class UserSeeds
{
    public static readonly UserEntity JohnDoe = new(
        Guid.Parse("f34cd643-1226-406d-971d-b5e6f745938e"),
        "John",
        "Doe",
        "737195090",
        "https://images.pexels.com/photos/771742/pexels-photo-771742.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500");

    public static readonly UserEntity ElonTusk = new(
        Guid.Parse("505b1e64-ed3c-44d1-883e-67de32b3ca59"),
        "Elon",
        "Tusk",
        "585453123",
        "https://pbs.twimg.com/profile_images/1443129819122782209/sqba2I3D_400x400.jpg"
    );

    static UserSeeds()
    {
        //JohnDoe.Vehicles.Add(VehicleSeeds.Felicia);
        //JohnDoe.ReceivedReviews.Add(ReviewSeeds.Perfect);
        //ElonTusk.Reservations.Add(ReservationSeeds.BrnoTwoSeats);
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasData(
            //JohnDoe with
            //{
            //    Reservations = Array.Empty<ReservationEntity>(),
            //},
            //ElonTusk with
            //{
            //    ReceivedReviews = Array.Empty<ReviewEntity>(),
            //    Vehicles = Array.Empty<VehicleEntity>(),
            //}
            JohnDoe, ElonTusk
        );
    }
}

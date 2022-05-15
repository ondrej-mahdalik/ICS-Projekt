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

    public static readonly UserEntity PavelNovak = new(
        Guid.Parse("606b1e64-ed3c-54a1-883e-69de32b3ca65"),
        "Pavel",
        "Novák",
        "737892123",
        null
    );

    public static readonly UserEntity JanNovotny = new(
        Guid.Parse("f34cb461-1234-406d-971d-b5e6f748938a"),
        "Jan",
        "Novotny",
        "783721984",
        "https://i.pinimg.com/custom_covers/222x/85498161615209203_1636332751.jpg"
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
            JohnDoe, ElonTusk, PavelNovak, JanNovotny
        );
    }
}

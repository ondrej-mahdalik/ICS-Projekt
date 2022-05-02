using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.DAL.Seeds;

public static class ReviewSeeds
{
    public static readonly ReviewEntity Perfect = new(
        Guid.Parse("e6364fdc-2f2a-46a4-bd7f-1016096801fd"),
        RideSeeds.PrahaBrno.Id,
        UserSeeds.JohnDoe.Id,
        5
    );

    public static readonly ReviewEntity Bad = new(
        Guid.Parse("4d809131-db36-47ed-8f95-3292ba018635"),
        RideSeeds.OlomoucOstrava.Id,
        UserSeeds.JanNovotny.Id,
        1
    );

    public static readonly ReviewEntity Average = new(
        Guid.Parse("62b2073d-4797-4548-9185-2aa54dc32956"),
        RideSeeds.OlomoucOstrava.Id,
        UserSeeds.PavelNovak.Id,
        2
    );

    public static readonly ReviewEntity BadBicycleRide = new(
        Guid.Parse("79cd988b-b2c3-450d-aa15-394eb8fbe808"),
        RideSeeds.TynecOlomouc.Id,
        UserSeeds.ElonTusk.Id,
        1
    );

    public static readonly ReviewEntity VanRideElonTusk = new(
        Guid.Parse("3d3d171d-0d6a-4d2f-a587-59ac49d56003"),
        RideSeeds.TynecPraha.Id,
        UserSeeds.ElonTusk.Id,
        0
    );

    public static readonly ReviewEntity VanRideJohnDoe = new(
        Guid.Parse("5ea15a61-49fa-4149-8cbf-5e3189fe1920"),
        RideSeeds.TynecPraha.Id,
        UserSeeds.JohnDoe.Id,
        1
    );

    public static readonly ReviewEntity VanRideJanNovotny = new(
        Guid.Parse("718dca06-27e2-4dbb-a882-90650d6398d8"),
        RideSeeds.TynecPraha.Id,
        UserSeeds.JanNovotny.Id,
        0
    );


    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewEntity>().HasData(
            Perfect, Bad, Average, BadBicycleRide, VanRideElonTusk, VanRideJohnDoe, VanRideJanNovotny
        );
    }
}

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

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewEntity>().HasData(
            Perfect, Bad, Average
        );
    }
}

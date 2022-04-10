using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.DAL.Seeds;

public static class ReviewSeeds
{
    public static readonly ReviewEntity Perfect = new(
        Guid.Parse("e6364fdc-2f2a-46a4-bd7f-1016096801fd"),
        Guid.Parse("42b612c1-b668-4168-9b73-71acfb64f094"),
        Guid.Parse("f34cd643-1226-406d-971d-b5e6f745938e"),
        5
    );

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewEntity>().HasData(
            Perfect
        );
    }
}

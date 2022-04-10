using System;
using Microsoft.EntityFrameworkCore;
using RideSharing.Common.Enums;
using RideSharing.DAL.Entities;

namespace RideSharing.DAL.Seeds;

public static class ReviewSeeds
{
    public static readonly ReviewEntity Perfect = new(
        Id: Guid.Parse(input: "e6364fdc-2f2a-46a4-bd7f-1016096801fd"),
        RideId: Guid.Parse(input: "42b612c1-b668-4168-9b73-71acfb64f094"),
        AuthorUserId: Guid.Parse(input: "f34cd643-1226-406d-971d-b5e6f745938e"),
        Rating: 5
    );

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewEntity>().HasData(
            Perfect
        );
    }
}

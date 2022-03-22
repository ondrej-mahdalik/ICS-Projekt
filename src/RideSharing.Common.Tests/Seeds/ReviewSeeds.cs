using System;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.DAL.Seeds;

public static class ReviewSeeds
{
    public static readonly ReviewEntity Perfect = new(
        Id: Guid.Parse(input: "e6364fdc-2f2a-46a4-bd7f-1016096801fd"),
        RideId: Guid.Parse(input: "42b612c1-b668-4168-9b73-71acfb64f094"),
        UserId: Guid.Parse(input: "505b1e64-ed3c-44d1-883e-67de32b3ca59"),
        Rating: 5
    );

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewEntity>().HasData(
            Perfect
        );
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.Common.Tests.Seeds;

public static class ReviewSeeds
{
    public static readonly ReviewEntity Perfect = new(
        Id: Guid.Parse(input: "3ad87324-4559-4db8-b918-206838fa33a1"),
        RideId: RideSeeds.PrahaBrno.Id,
        ReviewedUserId: UserSeeds.ElonTusk.Id,
        AuthorUserId: UserSeeds.JohnDoe.Id,
        Rating: 5
    );

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewEntity>().HasData(
            Perfect
        );
    }
}

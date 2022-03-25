using System;
using Microsoft.EntityFrameworkCore;
using RideSharing.Common.Enums;
using RideSharing.DAL.Entities;

namespace RideSharing.Common.Tests.Seeds;

public static class ReviewSeeds
{
    public static readonly ReviewEntity EmptyReviewEntity = new(
        Id: default,
        RideId: default,
        UserId: default,
        Rating: default
    );

    public static readonly ReviewEntity PerfectRatingReview = new(
        Id: Guid.Parse(input: "48E70EB0-279B-40C8-B6B0-E69D95C82BBB"),
        RideId: RideSeeds.PragueBrno.Id,
        UserId: UserSeeds.DriverUser.Id,
        Rating: RatingType.FiveStars
    );

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewEntity>().HasData(
            PerfectRatingReview
        );
    }
}

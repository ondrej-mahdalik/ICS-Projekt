using System;
using System.Net.Http.Headers;
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

    public static readonly ReviewEntity DriverBnoBratislavaReview = new(
        Id: Guid.Parse(input: "48E70EB0-279B-40C8-B6B0-E69D95C82BBB"),
        RideId: RideSeeds.BrnoBratislava.Id,
        UserId: UserSeeds.CreatedRidesUser.Id,
        Rating: RatingType.FiveStars
    )
    {
        User = UserSeeds.CreatedRidesUser,
        Ride = RideSeeds.BrnoBratislava
    };

    public static readonly ReviewEntity PassengerBnoBratislavaReview = new(
        Id: Guid.Parse(input: "729D672A-B397-4289-A9EF-F07E3762E993"),
        RideId: RideSeeds.BrnoBratislava.Id,
        UserId: UserSeeds.ReservationUser1.Id,
        Rating: RatingType.FourStars
    )
    {
        User = UserSeeds.ReservationUser1,
        Ride = RideSeeds.BrnoBratislava
    };

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewEntity>().HasData(
            DriverBnoBratislavaReview with {User = null, Ride = null},
            PassengerBnoBratislavaReview with { User = null, Ride = null }
        );
    }
}

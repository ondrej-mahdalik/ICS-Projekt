using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;

namespace RideSharing.Common.Tests.Seeds;

public static class ReviewSeeds
{
    public static readonly ReviewEntity EmptyReviewEntity = new(
        default,
        default,
        default,
        default
    );

    public static readonly ReviewEntity DriverPragueBrnoReview = new(
        Guid.Parse("48E70EB0-279B-40C8-B6B0-E69D95C82BBB"),
        RideSeeds.PragueBrno.Id,
        UserSeeds.ReservationUser1.Id,
        5
    );

    public static readonly ReviewEntity DriverAuthoredPragueBrnoReview = new(
        Guid.Parse("3609ffa7-6fde-4856-9519-656b37f58fd9"),
        RideSeeds.PragueBrno.Id,
        UserSeeds.DriverUser.Id,
        5
    );

    public static readonly ReviewEntity DeleteReview = new(
        Guid.Parse("b9887318-b964-4974-8fcc-3be131c7cca4"),
        RideSeeds.CascadeDeleteRide.Id,
        UserSeeds.CascadeDeleteUser.Id,
        1
    );

    public static readonly ReviewEntity JustSubmittedReview = new(
        Guid.Parse("cf8717b5-042a-435b-9fcd-4b1aa94a8309"),
        null,
        UserSeeds.JustSubmittedReviewUser.Id,
        1
    );

    public static readonly ReviewEntity JustRideReview = new(
        Guid.Parse("c24421b0-68f5-4f54-b160-1a170623bce1"),
        RideSeeds.JustReviewRide.Id,
        null,
        3
    );

    public static readonly ReviewEntity UpdateReview = GetNoRelationsEntity(DeleteReview) with
    {
        Id = Guid.Parse("22649857-2aa3-4c23-b1cd-7bbc054faa3a")
    };


    public static ReviewEntity GetNoRelationsEntity(ReviewEntity entity)
    {
        return entity with { Ride = null, AuthorUser = null };
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewEntity>()
            .HasData(
                DriverPragueBrnoReview,
                DriverAuthoredPragueBrnoReview,
                UpdateReview,
                DeleteReview,
                JustSubmittedReview,
                JustRideReview
            );
    }
}

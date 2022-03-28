using System;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using RideSharing.Common.Enums;
using RideSharing.DAL.Entities;

namespace RideSharing.Common.Tests.DALTestsSeeds;

public static class ReviewSeeds
{
    public static readonly ReviewEntity EmptyReviewEntity = new(
        Id: default,
        RideId: default,
        ReviewedUserId: default,
        AuthorUserId: default,
        Rating: default
    );

    public static readonly ReviewEntity DriverPragueBrnoReview = new(
        Id: Guid.Parse(input: "48E70EB0-279B-40C8-B6B0-E69D95C82BBB"),
        RideId: RideSeeds.PragueBrno.Id,
        AuthorUserId: UserSeeds.ReservationUser1.Id,
        ReviewedUserId: UserSeeds.DriverUser.Id,
        Rating: 5
    );

    public static readonly ReviewEntity DriverAuthoredPragueBrnoReview = new(
        Id: Guid.Parse(input: "3609ffa7-6fde-4856-9519-656b37f58fd9"),
        RideId: RideSeeds.PragueBrno.Id,
        AuthorUserId: UserSeeds.DriverUser.Id,
        ReviewedUserId: UserSeeds.ReservationUser1.Id,
        Rating: 5
    );

    public static readonly ReviewEntity DeleteReview = new(
        Id: Guid.Parse(input: "b9887318-b964-4974-8fcc-3be131c7cca4"),
        RideId: RideSeeds.CascadeDeleteRide.Id,
        AuthorUserId: UserSeeds.CascadeDeleteUser.Id,
        ReviewedUserId: UserSeeds.ReservationUser2.Id,
        Rating: 1
    );

    public static readonly ReviewEntity JustObtainedReview = new(
        Id: Guid.Parse(input: "a9d5d032-ccdb-4db6-96bb-71df214b7a7f"),
        RideId: null,
        AuthorUserId: null,
        ReviewedUserId: UserSeeds.JustObtainedReviewUser.Id,
        Rating: 1
    );

    public static readonly ReviewEntity JustSubmittedReview = new(
        Id: Guid.Parse(input: "cf8717b5-042a-435b-9fcd-4b1aa94a8309"),
        RideId: null,
        AuthorUserId: UserSeeds.JustSubmittedReviewUser.Id,
        ReviewedUserId: UserSeeds.CascadeDeleteUser.Id,
        Rating: 1
    );

    public static readonly ReviewEntity JustRideReview = new(
        Id: Guid.Parse(input: "c24421b0-68f5-4f54-b160-1a170623bce1"),
        RideId: RideSeeds.JustReviewRide.Id,
        AuthorUserId: null,
        ReviewedUserId: UserSeeds.CascadeDeleteUser.Id,
        Rating: 3
    );

    public static readonly ReviewEntity UpdateReview = GetNoRelationsEntity(DeleteReview) with
    {
        Id = Guid.Parse("22649857-2aa3-4c23-b1cd-7bbc054faa3a")
    };


    public static ReviewEntity GetNoRelationsEntity(ReviewEntity entity)
    {
        return entity with
        {
            Ride = null,
            ReviewedUser = null,
            AuthorUser = null
        };
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewEntity>()
            .HasData(
                DriverPragueBrnoReview,
                DriverAuthoredPragueBrnoReview,
                UpdateReview,
                DeleteReview,
                JustObtainedReview,
                JustSubmittedReview,
                JustRideReview
            );
    }
}

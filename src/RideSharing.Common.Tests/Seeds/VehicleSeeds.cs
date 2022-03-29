using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;
using RideSharing.Common.Enums;

namespace RideSharing.Common.Tests.Seeds;

public static class VehicleSeeds
{
    public static readonly VehicleEntity Felicia = new(
        Id: Guid.Parse(input: "e7320660-b9cf-49a3-b10e-388d8deba01a"),
        OwnerId: UserSeeds.ElonTusk.Id,
        VehicleType: VehicleType.Car,
        Make: "Škoda",
        Model: "Felicia",
        Registered: DateTime.Parse("05/16/1996", CultureInfo.InvariantCulture),
        Seats: 5,
        ImageUrl: "https://auta5p.eu/katalog/skoda/felicia_28.jpg");

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleEntity>().HasData(
            Felicia
        );
    }
}

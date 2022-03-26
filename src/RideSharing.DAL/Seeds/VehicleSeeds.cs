using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RideSharing.DAL.Entities;
using RideSharing.Common.Enums;

namespace RideSharing.DAL.Seeds;

public static class VehicleSeeds
{
    public static readonly VehicleEntity Felicia = new(
        Id: Guid.Parse(input: "0a5ef2a1-d541-45ac-82a3-6b63a24d0572"),
        OwnerId: Guid.Parse(input: "f34cd643-1226-406d-971d-b5e6f745938e"),
        Type: VehicleType.Car,
        Make: "Škoda",
        Model: "Felicia",
        Registered: DateTime.Parse("16.5.1996"),
        Seats: 5,
        ImageUrl: "https://auta5p.eu/katalog/skoda/felicia_28.jpg");

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleEntity>().HasData(
            Felicia
        );
    }
}

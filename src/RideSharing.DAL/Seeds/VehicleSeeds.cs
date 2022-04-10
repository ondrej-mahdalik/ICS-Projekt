using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RideSharing.Common.Enums;
using RideSharing.DAL.Entities;

namespace RideSharing.DAL.Seeds;

public static class VehicleSeeds
{
    public static readonly VehicleEntity Felicia = new(
        Guid.Parse("0a5ef2a1-d541-45ac-82a3-6b63a24d0572"),
        Guid.Parse("f34cd643-1226-406d-971d-b5e6f745938e"),
        VehicleType.Car,
        "Škoda",
        "Felicia",
        DateTime.Parse("08/05/1996", CultureInfo.InvariantCulture),
        5,
        "https://auta5p.eu/katalog/skoda/felicia_28.jpg");

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleEntity>().HasData(
            Felicia
        );
    }
}

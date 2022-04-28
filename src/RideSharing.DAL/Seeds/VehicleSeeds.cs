using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RideSharing.Common.Enums;
using RideSharing.DAL.Entities;

namespace RideSharing.DAL.Seeds;

public static class VehicleSeeds
{
    public static readonly VehicleEntity Felicia = new(
        Guid.Parse("0a5ef2a1-d541-45ac-82a3-6b63a24d0572"),
        UserSeeds.JohnDoe.Id,
        VehicleType.Car,
        "Škoda",
        "Felicia",
        DateTime.Parse("08/05/1996", CultureInfo.InvariantCulture),
        5,
        "https://auta5p.eu/katalog/skoda/felicia_28.jpg");

    public static readonly VehicleEntity Karosa = new(
        Guid.Parse("0a5af2a1-1234-45ac-12b3-6a63b24f0654"),
        UserSeeds.ElonTusk.Id,
        VehicleType.Bus,
        "Karosa",
        "C743",
        DateTime.Parse("03/02/1980", CultureInfo.InvariantCulture),
        20,
        "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a1/Jablonec_nad_Nisou%2C_autobusov%C3%A9_n%C3%A1dra%C5%BE%C3%AD%2C_bus_Karosa.jpg/800px-Jablonec_nad_Nisou%2C_autobusov%C3%A9_n%C3%A1dra%C5%BE%C3%AD%2C_bus_Karosa.jpg?20080920210004");

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleEntity>().HasData(
            Felicia, Karosa
        );
    }
}

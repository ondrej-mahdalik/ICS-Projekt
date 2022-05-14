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

    public static readonly VehicleEntity YBR125 = new(
        Guid.Parse("3a58f7bf-458a-4115-9c74-a7f0f0c4b7c7"),
        UserSeeds.JohnDoe.Id,
        VehicleType.Motorcycle,
        "Yamaha",
        "YBR125",
        DateTime.Parse("04/16/2004", CultureInfo.InvariantCulture),
        2,
        "http://img.motorkari.cz/upload/images/bazar/2022-04-0/8956720.jpg" );

    public static readonly VehicleEntity Bicycle = new(
        Guid.Parse("4202e1f6-81ff-4938-84a6-9398b162c69a"),
        UserSeeds.PavelNovak.Id,
        VehicleType.Bicycle,
        "Favorit",
        "F7",
        DateTime.Parse("06/11/1986", CultureInfo.InvariantCulture),
        2,
        "https://cdn.aukro.cz/images/sk1640886041602/730x548/jizdni-kolo-favorit-114653837.jpeg" );

    public static readonly VehicleEntity Van = new(
        Guid.Parse("3fa8a24e-a370-48ad-91f1-5f3713828d20"),
        UserSeeds.PavelNovak.Id,
        VehicleType.Van,
        "Volkswagen",
        "T1",
        DateTime.Parse("06/11/1968", CultureInfo.InvariantCulture),
        8,
        "https://www.bazos.cz/img/1/430/151570430.jpg");

    public static readonly VehicleEntity AudiA8 = new(
        Id: Guid.Parse("1dfd76ec-51cc-41a5-a133-99af4b87524e"),
        OwnerId: UserSeeds.PavelNovak.Id,
        VehicleType: VehicleType.Car,
        Make: "Audi",
        Model: "A8",
        Registered: DateTime.Parse("2000-04-04", CultureInfo.InvariantCulture),
        Seats: 5,
        ImageUrl: "https://i.ytimg.com/vi/W_KorO42ZkA/maxresdefault.jpg"
        );

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleEntity>().HasData(
            Felicia, Karosa, YBR125, Bicycle, Van, AudiA8
        );
    }
}

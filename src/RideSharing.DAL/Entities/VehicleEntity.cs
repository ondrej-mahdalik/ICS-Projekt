using System.Drawing;

namespace RideSharing.DAL.Entities
{
    public record VehicleEntity(Guid Id, Guid OwnerId, VehicleType Type, string Make, string Model, DateTime Registered,
        ushort Seats,
        string? ImageUrl) : IEntity
    {
        public UserEntity? Owner { get; init; }
    }

    public enum VehicleType
    {
        Bicycle,
        Motorcycle,
        Car,
        Van,
        Minibus,
        Bus
    }
}

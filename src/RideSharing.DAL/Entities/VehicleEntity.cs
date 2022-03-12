using RideSharing.Common.Enums;

namespace RideSharing.DAL.Entities
{
    public record VehicleEntity(Guid Id, VehicleType Type, string Make, string Model, DateTime Registered, ushort Seats,
        string? ImageUrl) : IEntity;
}

using RideSharing.Common.Enums;

namespace RideSharing.DAL.Entities;

public record VehicleEntity(
    Guid Id,
    Guid OwnerId,
    VehicleType VehicleType,
    string Make,
    string Model,
    DateTime Registered,
    ushort Seats,
    string? ImageUrl) : IEntity
{
#nullable disable
    public VehicleEntity() : this(default, default, default, string.Empty, string.Empty, default, default, default) { }
#nullable enable
    public UserEntity? Owner { get; init; }
    public ICollection<RideEntity> Rides { get; init; } = new List<RideEntity>();
}

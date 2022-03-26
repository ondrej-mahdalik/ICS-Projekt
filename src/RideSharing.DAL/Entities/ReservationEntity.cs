namespace RideSharing.DAL.Entities;

public record ReservationEntity(Guid Id, Guid UserId, Guid RideId, ushort Seats, DateTime Timestamp) : IEntity
{
    public UserEntity? User { get; init; }
    public RideEntity? Ride { get; init; }
}

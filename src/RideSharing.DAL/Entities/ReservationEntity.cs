namespace RideSharing.DAL.Entities
{
    public record ReservationEntity(Guid Id, Guid UserId, Guid RideId, ushort Seats, DateTime Timestamp) : IEntity
    {
        private UserEntity? User { get; init; }
        private RideEntity? Ride { get; init; }
    }
}

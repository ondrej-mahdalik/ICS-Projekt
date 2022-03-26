namespace RideSharing.DAL.Entities
{
    public record ReservationEntity(Guid Id, Guid ReservingUserId, Guid RideId, ushort Seats, DateTime Timestamp) : IEntity
    {
        public UserEntity? ReservingUser { get; init; }
        public RideEntity? Ride { get; init; }
    }
}

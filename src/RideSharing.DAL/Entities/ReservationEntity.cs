namespace RideSharing.DAL.Entities
{
    public record ReservationEntity(Guid Id, Guid UserId, ushort Seats, DateTime Timestamp) : IEntity
    {
        private UserEntity? User { get; init; }
    }
}

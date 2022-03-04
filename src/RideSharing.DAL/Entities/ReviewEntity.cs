namespace RideSharing.DAL.Entities
{
    public record ReviewEntity(Guid Id, Guid RideId, Guid UserId, ushort Rating) : IEntity
    {
        public RideEntity? Ride { get; init; }
        public UserEntity? User { get; init; }
    }
}

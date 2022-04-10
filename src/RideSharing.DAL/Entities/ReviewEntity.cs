namespace RideSharing.DAL.Entities;

public record ReviewEntity(
    Guid Id,
    Guid? RideId,
    Guid? AuthorUserId,
    ushort Rating) : IEntity
{
    public RideEntity? Ride { get; init; }
    public UserEntity? AuthorUser { get; init; }
}

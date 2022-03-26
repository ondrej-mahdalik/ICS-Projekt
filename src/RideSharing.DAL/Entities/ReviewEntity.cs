using RideSharing.Common.Enums;

namespace RideSharing.DAL.Entities;

public record ReviewEntity(Guid Id, Guid RideId, Guid UserId, RatingType Rating) : IEntity
{
    public RideEntity? Ride { get; init; }
    public UserEntity? User { get; init; }
}

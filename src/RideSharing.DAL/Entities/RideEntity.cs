namespace RideSharing.DAL.Entities;

public record RideEntity(
    Guid Id,
    int Distance,
    int SharedSeats,
    DateTime Departure,
    DateTime Arrival,
    Guid? VehicleId,
    string? Note,
    string FromName,
    string ToName
) : IEntity
{
    public VehicleEntity? Vehicle { get; init; }
    public ICollection<ReservationEntity> Reservations { get; init; } = new List<ReservationEntity>();
    public ICollection<ReviewEntity> Reviews { get; init; } = new List<ReviewEntity>();
}

namespace RideSharing.DAL.Entities;

public record UserEntity(
    Guid Id,
    string Name,
    string Surname,
    string Phone,
    string? ImageUrl) : IEntity
{
    public ICollection<ReviewEntity> SubmittedReviews { get; init; } = new List<ReviewEntity>();
    public ICollection<VehicleEntity> Vehicles { get; init; } = new List<VehicleEntity>();
    public ICollection<ReservationEntity> Reservations { get; init; } = new List<ReservationEntity>();
}

namespace RideSharing.DAL.Entities
{
    public record UserEntity(
        Guid Id, string Name, string Surname, string Phone, string ImageUrl) : IEntity
    {
        private ICollection<ReviewEntity> Reviews { get; init; } = new List<ReviewEntity>();
        private ICollection<VehicleEntity> Vehicles { get; init; } = new List<VehicleEntity>();
        private ICollection<ReservationEntity> Reservations { get; init; } = new List<ReservationEntity>();
    }
}

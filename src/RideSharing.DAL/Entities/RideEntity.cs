namespace RideSharing.DAL.Entities
{
    public record RideEntity(Guid Id, Location From, Location To, DateTime Departure, DateTime Arrival, Guid DriverId, string? Note) : IEntity
    {
        public UserEntity? Driver { get; init; }
        public ICollection<ReservationEntity> Reservations { get; init; } = new List<ReservationEntity>();
    }

    public struct Location
    {
        public string Name { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }
    }
}

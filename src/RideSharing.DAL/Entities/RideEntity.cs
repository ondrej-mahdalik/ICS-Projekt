namespace RideSharing.DAL.Entities
{
    public record RideEntity(Guid Id, string FromName, int SharedSeats, double FromLatitude, double FromLongitude, int Distance,
        string ToName, double ToLatitude, double ToLongitude, DateTime Departure, DateTime Arrival, Guid VehicleId,
        string? Note) : IEntity
    {
        public VehicleEntity? Vehicle { get; init; }
        public ICollection<ReservationEntity> Reservations { get; init; } = new List<ReservationEntity>();
        public ICollection<ReviewEntity> Reviews { get; init; } = new List<ReviewEntity>();
    }
}

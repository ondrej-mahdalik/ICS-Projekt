namespace RideSharing.DAL.Entity
{
    public class Ride
    {
        public Guid RideId { get; set; }
        public Location From { get; init; }
        public Location To { get; init; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }

        public Guid DriverId { get; set; }
        public User Driver { get; set; }

        public ICollection<User> Passengers { get; set; }

        public Review Review { get; set; }
        public string Note { get; set; }

    }

    public struct Location
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

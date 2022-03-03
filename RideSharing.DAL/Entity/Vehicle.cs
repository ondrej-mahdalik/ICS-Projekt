using System.Drawing;

namespace RideSharing.DAL.Entity
{
    public class Vehicle
    {
        public Guid VehicleId { get; set; }
        public VehicleType Type { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime Registered { get; set; }
        public ushort Seats { get; set; }
        public Image Image { get; set; }
    }

    public enum VehicleType
    {
        Bicycle,
        Motorcycle,
        Car,
        Van,
        Minibus,
        Bus
    }
}

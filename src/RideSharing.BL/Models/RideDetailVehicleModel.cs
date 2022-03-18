namespace RideSharing.BL.Models;

public record RideDetailVehicleModel(
    string Make,
    string Model,
    DateTime Registered) : ModelBase
{
    public string Make { get; set; } = Make;
    public string Model { get; set; } = Model;
    public DateTime Registered { get; set; } = Registered;
    public string? ImageUrl { get; set; }
    public int AvailableSeats { get; set; }
}

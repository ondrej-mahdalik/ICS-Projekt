namespace RideSharing.BL.Models;

public record RideDetailDriverModel(
    string Name,
    string Surname) : ModelBase
{
    public string Name { get; set; } = Name;
    public string Surname { get; set; } = Surname;
    public string? ImageUrl { get; set; }
    public int NumberOfReviews { get; set; }
}

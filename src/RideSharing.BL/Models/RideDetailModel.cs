using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record RideDetailModel(
    string FromName,
    double FromLatitude,
    double FromLongitude,
    string ToName,
    double ToLatitude,
    double ToLongitude,
    DateTime Departure,
    DateTime Arrival,
    Guid DriverId,
    Guid VehicleId) : ModelBase
{
    public string FromName { get; set; } = FromName;
    public double FromLatitude { get; set; } = FromLatitude;
    public double FromLongitude { get; set; } = FromLongitude;
    public string ToName { get; set; } = ToName;
    public double ToLatitude { get; set; } = ToLatitude;
    public double ToLongitude { get; set; } = ToLongitude;
    public DateTime Departure { get; set; } = Departure;
    public DateTime Arrival { get; set; } = Arrival;
    public Guid DriverId { get; set; } = DriverId;
    public Guid VehicleId { get; set; } = VehicleId;
    public string? Note { get; set; }
    public int Distance { get; set; }
    public TimeSpan Duration { get; set; }
    public List<UserDetailModel> Passengers { get; init; } = new();
    
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RideEntity, RideDetailModel>()
                .ReverseMap();
        }
    }
}

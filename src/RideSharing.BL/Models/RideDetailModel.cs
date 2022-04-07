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
    int Distance,
    int SharedSeats,
    DateTime Departure,
    DateTime Arrival,
    string? Note = null) : ModelBase
{
    public string FromName { get; set; } = FromName;
    public double FromLatitude { get; set; } = FromLatitude;
    public double FromLongitude { get; set; } = FromLongitude;
    public string ToName { get; set; } = ToName;
    public double ToLatitude { get; set; } = ToLatitude;
    public double ToLongitude { get; set; } = ToLongitude;
    
    public UserDetailModel? Driver { get; set; }
    public VehicleDetailModel? Vehicle { get; set; }
    
    public DateTime Departure { get; set; } = Departure;
    public DateTime Arrival { get; set; } = Arrival;
    public string? Note { get; set; } = Note;
    public int Distance { get; set; } = Distance;
    public int SharedSeats { get; set; } = SharedSeats;
    public TimeSpan Duration => Arrival - Departure;

    public List<RideDetailReservationModel> Reservations { get; init; } = new();
    
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RideEntity, RideDetailModel>()
                .ForMember(model => model.Duration, action => action.MapFrom(src => src.Arrival - src.Departure))
                .ForMember(model => model.Driver, action => action.MapFrom(src => src.Vehicle!.Owner))
                .ReverseMap();
        }
    }
}

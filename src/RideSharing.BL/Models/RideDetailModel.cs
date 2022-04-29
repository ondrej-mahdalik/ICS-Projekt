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
    public int Distance { get; set; } = Distance;
    public int SharedSeats { get; set; } = SharedSeats;
    public DateTime Departure { get; set; } = Departure;
    public DateTime Arrival { get; set; } = Arrival;
    public string? Note { get; set; } = Note;

    public List<ReservationDetailModel> Reservations { get; init; } = new();
    public VehicleListModel? Vehicle { get; set; }

    public TimeSpan Duration { get; set; } = Arrival - Departure;
    public int OccupiedSeats { get; set; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RideEntity, RideDetailModel>()
                .ForMember(entity => entity.Duration, action => action.Ignore())
                .ForMember(entity => entity.OccupiedSeats, action => action.Ignore())
                .ReverseMap()
                .ForMember(entity => entity.Vehicle, action => action.Ignore());


        }
    }
}

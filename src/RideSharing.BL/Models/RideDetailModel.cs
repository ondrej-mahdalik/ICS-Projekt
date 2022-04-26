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
    public VehicleListModel? Vehicle { get; set; }
    public DateTime Departure { get; set; } = Departure;
    public DateTime Arrival { get; set; } = Arrival;
    public string? Note { get; set; } = Note;
    public int Distance { get; set; } = Distance;
    public int SharedSeats { get; set; } = SharedSeats;

    public List<ReservationListModel> Reservations { get; init; } = new();

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RideEntity, RideDetailModel>()
                .ReverseMap()
                .ForMember(entity => entity.Vehicle, action => action.Ignore());
        }
    }
}

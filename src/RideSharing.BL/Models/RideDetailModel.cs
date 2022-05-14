using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record RideDetailModel(
    string FromName,
    string ToName,
    int Distance,
    int SharedSeats,
    DateTime Departure,
    DateTime Arrival,
    Guid VehicleId,
    string? Note = null) : ModelBase
{
    public string FromName { get; set; } = FromName;
    public string ToName { get; set; } = ToName;
    public int Distance { get; set; } = Distance;
    public int SharedSeats { get; set; } = SharedSeats;
    public DateTime Departure { get; set; } = Departure;
    public DateTime Arrival { get; set; } = Arrival;
    public string? Note { get; set; } = Note;

    public float DriverRating { get; set; }
    public int DriverReviewCount { get; set; }

    public List<ReservationDetailModel> Reservations { get; init; } = new();
    public Guid VehicleId { get; set; } = VehicleId;
    public VehicleListModel? Vehicle { get; set; }

    public TimeSpan Duration { get; set; } = Arrival - Departure;
    public int OccupiedSeats { get; set; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RideEntity, RideDetailModel>()
                .ForMember(model => model.Duration, action => action.Ignore())
                .ForMember(model => model.OccupiedSeats, action => action.Ignore())
                .ForMember(model => model.DriverRating, action => action.Ignore())
                .ForMember(model => model.DriverReviewCount, action => action.Ignore())
                .ReverseMap()
                .ForMember(entity => entity.Vehicle, action => action.Ignore())
                .ForMember(entity => entity.Reservations, action => action.Ignore());
        }
    }
}

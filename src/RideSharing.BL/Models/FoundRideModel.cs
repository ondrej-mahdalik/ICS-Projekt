using AutoMapper;
using RideSharing.Common.Enums;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record FoundRideModel(
    string FromName,
    string ToName,
    DateTime Departure,
    DateTime Arrival,
    int Distance,
    int SharedSeats) : ModelBase
{
    public string FromName { get; set; } = FromName;
    public string ToName { get; set; } = ToName;
    public DateTime Departure { get; set; } = Departure;
    public DateTime Arrival { get; set; } = Arrival;
    public int Distance { get; set; } = Distance;
    public int SharedSeats { get; set; } = SharedSeats;
    public TimeSpan Duration { get; set; }

    public float Rating { get; set; }
    public int ReviewCount { get; set; }

    public VehicleDetailModel? Vehicle { get; init; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RideEntity, FoundRideModel>()
                .ForMember(model => model.Duration, action => action.MapFrom(src => src.Arrival - src.Departure));
        }
    }
}

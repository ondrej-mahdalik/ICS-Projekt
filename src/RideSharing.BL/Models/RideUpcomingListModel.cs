using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record RideUpcomingListModel(
    string FromName,
    string ToName,
    DateTime Departure,
    DateTime Arrival,
    int SharedSeats) : ModelBase
{
    public string FromName { get; set; } = FromName;
    public string ToName { get; set; } = ToName;
    public DateTime Departure { get; set; } = Departure;
    public DateTime Arrival { get; set; } = Arrival;
    public int SharedSeats { get; set; } = SharedSeats;
    public int OccupiedSeats { get; set; }
    public bool IsDriver { get; set; }
    public VehicleListModel? Vehicle { get; set; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RideEntity, RideUpcomingListModel>()
                .ForMember(entity => entity.OccupiedSeats, action => action.Ignore())
                .ForMember(entity => entity.IsDriver, action => action.Ignore());


        }
    }
}

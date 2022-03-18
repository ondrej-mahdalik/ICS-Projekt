using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record RideListModel(
    string FromName,
    string ToName,
    DateTime Departure,
    DateTime Arrival) : ModelBase
{
    public string FromName { get; set; } = FromName;
    public string ToName { get; set; } = ToName;
    public DateTime Departure { get; set; } = Departure;
    public DateTime Arrival { get; set; } = Arrival;
    public int Distance { get; set; }
    public TimeSpan Duration { get; set; }
    
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RideEntity, RideListModel>();
        }
    }
}

using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record RideRecentListModel(
    string FromName,
    string ToName,
    DateTime Departure,
    DateTime Arrival) : ModelBase
{
    public string FromName { get; set; } = FromName;
    public string ToName { get; set; } = ToName;
    public DateTime Departure { get; set; } = Departure;
    public DateTime Arrival { get; set; } = Arrival;
    public bool IsDriver { get; set; }
    public bool HasReviewed { get; set; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RideEntity, RideRecentListModel>()
                .ForMember(entity => entity.IsDriver, action => action.Ignore())
                .ForMember(entity => entity.HasReviewed, action => action.Ignore());


        }
    }
}



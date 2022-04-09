using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record UserListModel(
    string Name,
    string Surname,
    string Phone,
    string? ImageUrl = null) : ModelBase
{
    public string Name { get; set; } = Name;
    public string Surname { get; set; } = Surname;
    public string Phone { get; set; } = Phone;
    public string? ImageUrl { get; set; } = ImageUrl;
    public int NumberOfVehicles { get; }
    public int UpcomingRidesCount { get; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserEntity, UserListModel>()
                .ForMember(dst => dst.NumberOfVehicles,
                    action => action.MapFrom(src => src.Vehicles.Count))
                .ForMember(dst => dst.UpcomingRidesCount,
                    action => action.MapFrom(src =>
                        src.Vehicles.SelectMany(x => x.Rides).Count(x => x.Departure > DateTime.Now) +
                        src.Reservations.Count(x => x.Ride != null && x.Ride.Departure > DateTime.Now)));
        }
    }
}

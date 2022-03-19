using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record UserListModel(
    string Name,
    string Surname) : ModelBase
{
    public string Name { get; set; } = Name;
    public string Surname { get; set; } = Surname;
    public string? ImageUrl { get; set; }
    public int NumberOfVehicles { get; init; }
    public List<ReviewDetailModel> Reviews { get; set; } = new();
    public int UpcomingRidesCount { get; init; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserEntity, UserListModel>()
                .ForMember(dst => dst.NumberOfVehicles,
                    action => action.MapFrom(src => src.Vehicles.Count))
                .ForMember(dst => dst.UpcomingRidesCount,
                    action => action.MapFrom(src =>
                        src.CreatedRides.Count(x => x.Departure > DateTime.Now) +
                        src.Reservations.Count(x => x.Ride != null && x.Ride.Departure > DateTime.Now)));
        }
    }
}

using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record UserListModel(
    string Name,
    string Surname,
    string Phone,
    string? ImageUrl = null) : ModelBase
{
    public string FullName { get;} = Name + " " + Surname;
    public string Phone { get; set; } = Phone;
    public string? ImageUrl { get; set; } = ImageUrl;
    public int NumberOfVehicles { get; set; }
    public int UpcomingRidesCount { get; set; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserEntity, UserListModel>()
                .ForMember(dst => dst.NumberOfVehicles, action => action.Ignore())
                .ForMember(dst => dst.UpcomingRidesCount, action => action.Ignore());
        }
    }
}

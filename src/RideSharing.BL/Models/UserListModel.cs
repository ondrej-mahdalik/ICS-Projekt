using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record UserListModel(
    string Name,
    string Surname,
    string? ImageUrl = null) : ModelBase
{
    public string Name { get; set; } = Name;
    public string Surname { get; set; } = Surname;
    public string? ImageUrl { get; set; } = ImageUrl;
    public int NumberOfVehicles { get; set; }
    public int UpcomingRidesCount { get; set; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserEntity, UserListModel>()
                .ForMember(entity => entity.NumberOfVehicles, action => action.Ignore())
                .ForMember(entity => entity.UpcomingRidesCount, action => action.Ignore());
                
        }
    }
}

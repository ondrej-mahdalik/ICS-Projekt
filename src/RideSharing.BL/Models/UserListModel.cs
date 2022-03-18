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
    
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserEntity, UserListModel>()
                .ForMember(dst => dst.NumberOfVehicles,
                    action => action.MapFrom(src => src.Vehicles.Count));
        }
    }
}

using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record UserDetailModel(
    string Name,
    string Surname,
    string Phone) : ModelBase
{
    public string Name { get; set; } = Name;
    public string Surname { get; set; } = Surname;
    public string Phone { get; set; } = Phone;
    public string? ImageUrl { get; set; }
    public int NumberOfVehicles { get; set; }
    public List<ReviewDetailModel> Rating{ get; set; } = new();

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserEntity, UserDetailModel>()
                .ForMember(dst => dst.NumberOfVehicles,
                    action => action.MapFrom(src => src.Vehicles.Count))
                .ReverseMap();
        }
    }
}

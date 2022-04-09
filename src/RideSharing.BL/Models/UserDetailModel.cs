using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record UserDetailModel(
    string Name,
    string Surname,
    string Phone,
    string? ImageUrl = null) : ModelBase
{
    public string Name { get; set; } = Name;
    public string Surname { get; set; } = Surname;
    public string Phone { get; set; } = Phone;
    public string? ImageUrl { get; set; } = ImageUrl;
    public List<VehicleListModel> Vehicles { get; set; } = new();
    public List<ReviewListModel> SubmittedReviews { get; set; } = new();
    
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserEntity, UserDetailModel>()
                .ReverseMap()
                .ForMember(entity => entity.Vehicles, action => action.Ignore())
                .ForMember(entity => entity.SubmittedReviews, action => action.Ignore());
        }
    }
}

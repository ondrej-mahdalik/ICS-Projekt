using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record ReviewDetailModel(ushort Rating) : ModelBase
{
    public ushort Rating { get; set; } = Rating;
    public RideRecentListModel? Ride { get; set; }
    public UserListModel? AuthorUser { get; set; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ReviewEntity, ReviewDetailModel>()
                .ReverseMap()
                .ForMember(entity => entity.Ride, action => action.Ignore())
                .ForMember(entity => entity.AuthorUser, action => action.Ignore());
        }
    }
}

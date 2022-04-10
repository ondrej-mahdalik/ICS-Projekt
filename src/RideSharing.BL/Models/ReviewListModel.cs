using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record ReviewListModel(ushort Rating) : ModelBase
{
    public ushort Rating { get; set; } = Rating;
    public RideListModel? Ride { get; set; }
    public UserListModel? AuthorUser { get; set; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ReviewEntity, ReviewListModel>();
        }
    }
}

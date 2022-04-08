using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record ReviewDetailModel(ushort Rating) : ModelBase
{
    public ushort Rating { get; set; } = Rating;
    public RideDetailModel? Ride { get; set; }
    public UserDetailModel? AuthorUser { get; set; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ReviewEntity, ReviewDetailModel>()
                .ReverseMap();
        }
    }
}

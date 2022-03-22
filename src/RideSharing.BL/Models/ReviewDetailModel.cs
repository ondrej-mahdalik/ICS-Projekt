using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record ReviewDetailModel(ushort Rating, Guid ReviewedUserId, Guid AuthorUserId, Guid RideId) : ModelBase
{
    public ushort Rating { get; set; } = Rating;
    public Guid ReviewedUserId { get; set; } = ReviewedUserId;
    public Guid AuthorUserId { get; set; } = AuthorUserId;
    public Guid RideId { get; set; } = RideId;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ReviewEntity, ReviewDetailModel>()
                .ReverseMap();
        }
    }
}

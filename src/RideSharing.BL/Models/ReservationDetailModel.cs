using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record ReservationDetailModel(DateTime Timestamp, ushort Seats, Guid ReservingUserId, Guid RideId) : ModelBase
{
    public Guid ReservingUserId { get; set; } = ReservingUserId;
    public Guid RideId { get; set; } = RideId;

    public UserDetailModel? ReservingUser { get; set; }
    public RideDetailModel? Ride { get; set; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ReservationEntity, ReservationDetailModel>()
                .ReverseMap()
                .ForMember(entity => entity.ReservingUser, action => action.Ignore())
                .ForMember(entity => entity.Ride, action => action.Ignore());
        }
    }
}

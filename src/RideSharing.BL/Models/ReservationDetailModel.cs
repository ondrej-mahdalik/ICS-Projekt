using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record ReservationDetailModel(DateTime Timestamp, ushort Seats) : ModelBase
{
    public UserDetailModel? ReservingUser { get; set; }
    public RideDetailModel? Ride { get; set; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ReservationEntity, ReservationDetailModel>()
                .ReverseMap()
                .ForMember(entity => entity.ReservingUserId,
                    action => action.MapFrom(model =>
                        model.ReservingUser == null ? Guid.Empty : model.ReservingUser.Id))
                .ForMember(entity => entity.RideId,
                    action => action.MapFrom(model => model.Ride == null ? Guid.Empty : model.Ride.Id))
                .ForMember(entity => entity.ReservingUser, action => action.Ignore())
                .ForMember(entity => entity.Ride, action => action.Ignore());
        }
    }
}

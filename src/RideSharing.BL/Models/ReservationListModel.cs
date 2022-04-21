using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record ReservationListModel(DateTime Timestamp, ushort Seats) : ModelBase
{
    public UserDetailModel? ReservingUser { get; set; }
    public RideDetailModel? Ride { get; set; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ReservationEntity, ReservationListModel>();
        }
    }
}

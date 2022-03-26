using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record RideDetailReservationModel(DateTime Timestamp, ushort Seats) : ModelBase
{
    public UserDetailModel? ReservingUser { get; set; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ReservationEntity, RideDetailReservationModel>();
        }
    }
}

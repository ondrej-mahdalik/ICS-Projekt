using AutoMapper;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record RideDetailReservationModel(DateTime Timestamp, ushort Seats) : ModelBase
{
    public UserDetailModel? User { get; set; }
    public RideDetailModel? Ride { get; set; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ReservationEntity, RideDetailReservationModel>();
        }
    }
}

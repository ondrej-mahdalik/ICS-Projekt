using AutoMapper;
using RideSharing.Common.Enums;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record RideListVehicleModel(ushort Seats, VehicleType VehicleType) : ModelBase
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<VehicleEntity, RideListVehicleModel>();
        }
    }
}

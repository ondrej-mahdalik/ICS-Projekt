using AutoMapper;
using RideSharing.Common.Enums;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record RideListVehicleModel(ushort Seats, VehicleType VehicleType) : ModelBase
{
    public ushort Seats { get; set; } = Seats;
    public VehicleType VehicleType { get; set; } = VehicleType;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<VehicleEntity, RideListVehicleModel>();
        }
    }
}

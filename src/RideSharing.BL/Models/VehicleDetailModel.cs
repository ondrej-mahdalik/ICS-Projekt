using AutoMapper;
using RideSharing.Common.Enums;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record VehicleDetailModel(
    Guid OwnerId,
    VehicleType Type,
    string Make,
    string Model,
    DateTime Registered,
    ushort Seats) : ModelBase
{
    public Guid OwnerId { get; set; } = OwnerId;
    public VehicleType Type { get; set; } = Type;
    public string Make { get; set; } = Make;
    public string Model { get; set; } = Model;
    public DateTime Registered { get; set; } = Registered;
    public ushort Seats { get; set; } = Seats;
    public string? ImageUrl { get; set; }
    
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<VehicleEntity, VehicleDetailModel>();
        }
    }
}

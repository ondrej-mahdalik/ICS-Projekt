using AutoMapper;
using RideSharing.Common.Enums;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record VehicleListModel(
    Guid OwnerId,
    VehicleType VehicleType,
    string Make,
    string Model,
    DateTime Registered,
    ushort Seats,
    string? ImageUrl = null) : ModelBase
{
    public Guid OwnerId { get; set; } = OwnerId;
    public VehicleType VehicleType { get; set; } = VehicleType;
    public string Make { get; set; } = Make;
    public DateTime Registered { get; set; } = Registered;
    public ushort Seats { get; set; } = Seats;
    public string? ImageUrl { get; set; } = ImageUrl;
    
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<VehicleEntity, VehicleListModel>();
        }
    }
}

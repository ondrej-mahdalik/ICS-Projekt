using AutoMapper;
using RideSharing.Common.Enums;
using RideSharing.DAL.Entities;

namespace RideSharing.BL.Models;

public record VehicleDetailModel(
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
    public string Model { get; set; } = Model;
    public DateTime Registered { get; set; } = Registered;
    public ushort Seats { get; set; } = Seats;
    public string? ImageUrl { get; set; } = ImageUrl;

    public UserListModel? Owner { get; init; }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<VehicleEntity, VehicleDetailModel>()
                .ReverseMap()
                .ForMember(entity => entity.Owner, action => action.Ignore());
        }
    }

    public static VehicleDetailModel Empty => new(default, default, string.Empty, string.Empty, DateTime.Now, default);
}

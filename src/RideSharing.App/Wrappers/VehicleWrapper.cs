using System;
using RideSharing.BL.Models;
using RideSharing.Common.Enums;

namespace RideSharing.App.Wrappers;

public class VehicleWrapper : ModelWrapper<VehicleDetailModel>
{
    public VehicleWrapper(VehicleDetailModel model) : base(model)
    {
    }

    public Guid? OwnerId
    {
        get => GetValue<Guid>();
        set => SetValue(value);
    }

    public VehicleType VehicleType
    {
        get => GetValue<VehicleType>();
        set => SetValue(value);
    }

    public string? Make         // TODO : Validate Make and Model
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? Model
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public DateTime Registered
    {
        get => GetValue<DateTime>();
        set => SetValue(value);
    }

    public ushort Seats
    {
        get => GetValue<ushort>();
        set => SetValue(value);
    }

    public string? ImageUrl
    {
        get => GetValue<string?>();
        set => SetValue(value); 
    }

    private void InitializeCollectionProperties(VehicleDetailModel model)
    {
        if( model.Rides == null)
        {
            throw new ArgumentException("VehicleDetailModel cannot be null");
        }
    }

    public static implicit operator VehicleWrapper(VehicleDetailModel detailModel)
            => new(detailModel);

    public static implicit operator VehicleDetailModel(VehicleWrapper wrapper)
        => wrapper.ThisModel;

}

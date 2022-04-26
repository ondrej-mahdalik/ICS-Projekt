using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public string? OwnerName
    {
        get => GetValue<string>();
    }

    public VehicleType VehicleType
    {
        get => GetValue<VehicleType>();
        set => SetValue(value);
    }

    public string? Make        
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

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Make))
        {
            yield return new ValidationResult($"{nameof(Make)} is required", new[] { nameof(Make) });
        }

        if (string.IsNullOrWhiteSpace(Model))
        {
            yield return new ValidationResult($"{nameof(Make)} is required", new[] { nameof(Make) });
        }
    }

    public static implicit operator VehicleWrapper(VehicleDetailModel detailModel)
            => new(detailModel);

    public static implicit operator VehicleDetailModel(VehicleWrapper wrapper)
        => wrapper.ThisModel;

}

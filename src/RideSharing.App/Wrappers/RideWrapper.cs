using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using RideSharing.App.Extensions;
using RideSharing.BL.Models;

namespace RideSharing.App.Wrappers;

public class RideWrapper : ModelWrapper<RideDetailModel>
{
    public RideWrapper(RideDetailModel model) : base(model)
    {
        InitializeCollectionProperties(model);
    }

    public string? FromName
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public double FromLatitude
    {
        get => GetValue<double>();
        set => SetValue(value);
    }

    public double FromLongitude
    {
        get => GetValue<double>();
        set => SetValue(value);
    }

    public string? ToName
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public double ToLatitude
    {
        get => GetValue<double>();
        set => SetValue(value);
    }

    public double ToLongitude
    {
        get => GetValue<double>();
        set => SetValue(value);
    }

    public int Distance
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public int SharedSeats
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public DateTime Departure
    {
        get => GetValue<DateTime>();
        set => SetValue(value);
    }

    public DateTime Arrival
    {
        get => GetValue<DateTime>();
        set => SetValue(value);
    }

    public int OccupiedSeats
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public string? Note
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public ObservableCollection<ReservationWrapper> Reservations { get; set; } = new();

    private void InitializeCollectionProperties(RideDetailModel model)
    {
        if(model.Reservations != null)
        {
            Reservations.AddRange(model.Reservations.Select(e => new ReservationWrapper(e)));
            RegisterCollection(Reservations, model.Reservations);
        }
    }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(FromName))
        {
            yield return new ValidationResult($"{nameof(FromName)} is required", new[] { nameof(FromName) });
        }
        if (FromLatitude == default)
        {
            yield return new ValidationResult($"{nameof(FromLatitude)} is required", new[] { nameof(FromLatitude) });
        }
        if (FromLongitude == default)
        {
            yield return new ValidationResult($"{nameof(FromLongitude)} is required", new[] { nameof(FromLongitude) });
        }
        if (string.IsNullOrWhiteSpace(ToName))
        {
            yield return new ValidationResult($"{nameof(ToName)} is required", new[] { nameof(ToName) });
        }
        if (ToLatitude == default)
        {
            yield return new ValidationResult($"{nameof(ToLatitude)} is required", new[] { nameof(ToLatitude) });
        }
        if (ToLongitude == default)
        {
            yield return new ValidationResult($"{nameof(ToLongitude)} is required", new[] { nameof(ToLongitude) });
        }
        if (SharedSeats <= 0)
        {
            yield return new ValidationResult($"{nameof(ToLongitude)} has to be positive", new[] { nameof(SharedSeats) });
        }
        if(Departure == default)
        {
            yield return new ValidationResult($"{nameof(Departure)} is required", new[] { nameof(Departure) });

        }
        if (Arrival == default)
        {
            yield return new ValidationResult($"{nameof(Arrival)} is required", new[] { nameof(Arrival) });

        }
    }

    public static implicit operator RideWrapper(RideDetailModel detailModel)
        => new(detailModel);

    public static implicit operator RideDetailModel(RideWrapper wrapper)
        => wrapper.ThisModel;
}

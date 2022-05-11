using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using RideSharing.App.Extensions;
using RideSharing.BL.Models;

namespace RideSharing.App.Wrappers;

public class UserWrapper : ModelWrapper<UserDetailModel>
{
    public UserWrapper(UserDetailModel? model) : base(model)
    {
        InitializeCollectionProperties(model);
    }

    public string? Name
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? Surname
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? Phone
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string? ImageUrl
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public ObservableCollection<VehicleWrapper> Vehicles { get; set; } = new();
    public ObservableCollection<ReviewWrapper> SubmittedReviews { get; set; } = new();

    private void InitializeCollectionProperties(UserDetailModel? model)
    {
        if (model is null)
            return;

        if(model.Vehicles.Count > 0)
        {
            Vehicles.AddRange(model.Vehicles.Select(e => new VehicleWrapper(e)));
            RegisterCollection(Vehicles, model.Vehicles);
        }
        if (model.SubmittedReviews.Count > 0)
        {
            SubmittedReviews.AddRange(model.SubmittedReviews.Select(e => new ReviewWrapper(e)));
            RegisterCollection(SubmittedReviews, model.SubmittedReviews);
        }
       
    }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        Regex letters = new Regex(@"^\p{L}+$");
        if (!letters.IsMatch(Name))
        {
            yield return new ValidationResult($"{nameof(Name)} is required", new[] { nameof(Name) });
        }
        if (!letters.IsMatch(Surname))
        {
            yield return new ValidationResult($"{nameof(Surname)} is required", new[] { nameof(Surname) });
        }

        Regex phoneFormat = new Regex(@"^\+?[\d \-]{7}[\d \-]*$");
        if (!phoneFormat.IsMatch(Phone))
        {
            yield return new ValidationResult($"{nameof(Phone)} is invalid", new[] { nameof(Phone) });
        }
    }

    public static implicit operator UserWrapper(UserDetailModel? detailModel)
        => new(detailModel);

    public static implicit operator UserDetailModel(UserWrapper wrapper)
        => wrapper.ThisModel;
}

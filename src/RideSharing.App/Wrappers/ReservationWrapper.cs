using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using RideSharing.App.Extensions;
using RideSharing.BL.Models;

namespace RideSharing.App.Wrappers;

public class ReservationWrapper : ModelWrapper<ReservationDetailModel>
{
    public ReservationWrapper(ReservationDetailModel model) : base(model)
    {

    }

    public Guid ReservingUserId
    {
        get => GetValue<Guid>();
        set => SetValue(value);
    }

    public string? ReservingUserName
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public Guid RideId
    {
        get => GetValue<Guid>();
        set => SetValue(value);
    }

    public ushort Seats
    {
        get => GetValue<ushort>();
        set => SetValue(value);
    }


    public static implicit operator ReservationWrapper(ReservationDetailModel detailModel)
        => new(detailModel);

    public static implicit operator ReservationDetailModel(ReservationWrapper wrapper)
        => wrapper.ThisModel;
}

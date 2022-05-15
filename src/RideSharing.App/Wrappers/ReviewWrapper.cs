using System;
using RideSharing.BL.Models;

namespace RideSharing.App.Wrappers;

public class ReviewWrapper : ModelWrapper<ReviewDetailModel>
{
    public ReviewWrapper(ReviewDetailModel model) : base(model)
    {
    }

    public Guid? RideId
    {
        get => GetValue<Guid>();
        set => SetValue(value);
    }

    public Guid? AuthorUserId
    {
        get => GetValue<Guid>();
        set => SetValue(value);
    }

    public ushort Rating
    {
        get => GetValue<ushort>();
        set => SetValue(value);
    }

    public static implicit operator ReviewWrapper(ReviewDetailModel detailModel)
            => new(detailModel);

    public static implicit operator ReviewDetailModel(ReviewWrapper wrapper)
        => wrapper.ThisModel;

}
